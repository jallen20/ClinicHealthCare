using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Models;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    public sealed partial class AppointmentDetailedDialog : ContentDialog
    {
        public DateTimeOffset AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }
        public int AppointmentId { get; set; }
        public bool ChangesMade { get; set; }
        public User User { get; set; }

        private bool editing;

        private AppointmentViewModel viewModel;

        public AppointmentDetailedDialog()
        {
            this.InitializeComponent();
            this.ChangesMade = false;
            this.DataContext = this;
            this.viewModel = new AppointmentViewModel();
            this.editing = false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.AppointmentDate.Date.CompareTo(DateTime.Now) > 0)
                
            if (this.User.Role.Equals("Nurse"))
            {
                this.editing = true;
                if (this.editing)
                {
                    this.Editing(this.editing);
                    this.done.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.IsPrimaryButtonEnabled = false;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Editing(false);
        }

        private void Editing(bool enabled)
        {
            this.pId.IsEnabled = this.pName.IsEnabled = this.dId.IsEnabled =
                this.dName.IsEnabled = this.date.IsEnabled = this.descr.IsEnabled = this.time.IsEnabled = enabled;
            this.Closing += ((sender, args) => { args.Cancel = enabled; });
            this.IsPrimaryButtonEnabled = !enabled;
        }

        private async void finishEdit(object sender, RoutedEventArgs e)
        {
            
           try {
                var doctorDal = new DoctorDAL();
                var patientDal = new PatientDAL();
                var pid = !string.IsNullOrWhiteSpace(this.PatientId)
                    ? int.Parse(this.PatientId)
                    : throw new ArgumentException("The patient id cannot be empty.");
                var did = !string.IsNullOrWhiteSpace(this.DoctorId)
                    ? int.Parse(this.DoctorId)
                    : throw new ArgumentException("The doctor id cannot be empty.");
                var date = this.AppointmentDate.Date.AddHours(this.AppointmentTime.Hours)
                               .AddMinutes(this.AppointmentTime.Minutes);

                var description = !string.IsNullOrWhiteSpace(this.Description)
                    ? this.Description
                    : throw new ArgumentException("The description cannot be null");
                var patient = await patientDal.GetPatientById(pid);
                var doctor = await doctorDal.GetDoctorByDoctorId(did);

                var appointment = new Appointment(patient, doctor, date, description)
                {
                    AppointmentId = this.AppointmentId
                };
                await this.viewModel.UpdateAppointment(appointment);
                this.ChangesMade = true;
            }
            catch (Exception ex)
            {
                var mes = new MessageDialog(ex.Message);
                await mes.ShowAsync();
                return;
            }
            this.Editing(false);
           }
    }
}
