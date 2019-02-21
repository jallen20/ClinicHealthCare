using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminSearchPage : Page
    {
        private AdminSearchViewModel viewModel;
        private User User { get; set; }
        public AdminSearchPage()
        {
            this.InitializeComponent();
            this.viewModel = new AdminSearchViewModel();
            this.DataContext = this.viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ring.IsActive = true;
            this.User = (User)Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            this.txtId.Text = this.User.Id;
            await this.viewModel.LoadAppointments();
            await this.viewModel.LoadCheckups();
            this.ring.IsActive = false;
        }

        private async void viewCheckup(object sender, ItemClickEventArgs e)
        {
            var checkup = e.ClickedItem as RoutineCheck;
            var dialog = new CheckupDetailedView()
            {
                AppointmentID = checkup.Appointment.AppointmentId,
                CheckupDate = (DateTimeOffset)checkup.Date,
                CheckupTime = checkup.Date.TimeOfDay,
                PatientId = checkup.Patient.Id,
                PatientName = checkup.Patient.FullName,
                NurseId = checkup.Nurse.NurseId,
                NurserName = checkup.Nurse.FullName,
                SystolicReading = checkup.SystolicReading,
                DiastolicBloodPressure = checkup.DiastolicBloodPressure,
                Weight = checkup.Weight,
                Temperature = checkup.Temperature,
                InitialDiagnosis = checkup.InitialDiagnosis,
                User = this.User
            };
            var result = await dialog.ShowAsync();
        }

        private void labTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private async void viewAppointment(object sender, ItemClickEventArgs e)
        {
            var appnt = e.ClickedItem as Appointment;
            var dialog = new AppointmentDetailedDialog()
            {
                AppointmentId = appnt.AppointmentId,
                AppointmentDate = (DateTimeOffset)appnt?.Date,
                AppointmentTime = appnt.Date.TimeOfDay,
                PatientId = appnt.Patient.Id.ToString(),
                PatientName = appnt.Patient.FullName,
                DoctorId = appnt.Doctor.DoctorId.ToString(),
                DoctorName = appnt.Doctor.FullName,
                Description = appnt.Description,
                User = this.User
            };
            var result = await dialog.ShowAsync();

        }

        private async void search(object sender, RoutedEventArgs e)
        {

            this.ring.IsActive = true;
            await this.viewModel.Search(this.startDate.Date.Date, this.endDate.Date.Date, this.appointments_ListView, this.checkups_ListView);
            this.ring.IsActive = false;
        }
    }
}
