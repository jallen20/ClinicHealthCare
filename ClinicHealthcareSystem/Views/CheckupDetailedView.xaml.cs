using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.ViewModels;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    public sealed partial class CheckupDetailedView : ContentDialog
    {
        public int AppointmentID { get; set; }
        public DateTimeOffset CheckupDate { get; set; }
        public TimeSpan CheckupTime { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int NurseId { get; set; }
        public string NurserName { get; set; }
        public double SystolicReading { get; set; }
        public double DiastolicBloodPressure { get; set; }
        public double Weight { get; set; }
        public double Temperature { get; set; }
        public string InitialDiagnosis { get; set; }
        public bool ChangesMade { get; set; }
        public User User { get; set; }
        public CheckupDetailedView()
        {
            this.InitializeComponent();
            this.ChangesMade = false;
        }



        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.User.Role.Equals("Nurse"))
                this.Editing(true);
            else this.IsPrimaryButtonEnabled = false;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           this.Editing(false);
        }

        private void Editing(bool editing)
        {
            this.date.IsEnabled = this.time.IsEnabled = this.pId.IsEnabled =
                this.pName.IsEnabled = this.dId.IsEnabled = this.sReading.IsEnabled =
                    this.bp.IsEnabled = this.weight.IsEnabled = this.temp.IsEnabled
                        = this.init.IsEnabled = this.done.IsEnabled = editing;
            this.Closing += ((sender, args) => { args.Cancel = editing; });
            this.IsPrimaryButtonEnabled = !editing;

        }

        private async void finishEdit(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ring.IsActive = true;
                var patientDal = new PatientDAL();
                var nurseDal = new NurseDAL();
                var appointDal = new AppointmentDAL();
                var vm = new RoutineCheckupViewModel();
                var appnt = await appointDal.GetAppointmentById(this.AppointmentID);
                var patient = await patientDal.GetPatientById(this.PatientId);
                var nurse = await nurseDal.GetNurseByNurseId(this.NurseId);
                var date = this.CheckupDate.Date.AddHours(this.CheckupTime.Hours).AddMinutes(this.CheckupTime.Minutes);
                var sys = !string.IsNullOrWhiteSpace(this.SystolicReading.ToString())
                    ? this.SystolicReading
                    : throw new ArgumentException("The Systolic Reading cannot be empty.");
                var dbp = !string.IsNullOrWhiteSpace(this.DiastolicBloodPressure.ToString())
                    ? this.DiastolicBloodPressure
                    : throw new ArgumentException("The blood pressure cannot be empty.");
                var weght = !string.IsNullOrWhiteSpace(this.Weight.ToString())
                    ? this.Weight
                    : throw new ArgumentException("The weight cannot be empty");
                var tep = !string.IsNullOrWhiteSpace(this.Temperature.ToString())
                    ? this.Temperature
                    : throw new ArgumentException("The tmeperature cannot be empty");
                var diag = !string.IsNullOrWhiteSpace(this.InitialDiagnosis)
                    ? this.InitialDiagnosis
                    : throw new ArgumentException("The initial diaggnosis cannot be empty");
                var checkup = new RoutineCheck(appnt, nurse, patient, date, sys, dbp, weght, tep, diag);
                await vm.EditCheckup(checkup);
                this.Editing(false);
                this.ChangesMade = true;
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                this.error.Text = ex.Message;
                await Task.Delay(4000);
                this.error.Text = "";
            }

            this.ring.IsActive = false;
        }
    }
}
