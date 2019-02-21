using ClinicHealthcareSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.Views.Utility;
using AppointmentViewModel = ClinicHealthcareSystem.ViewModels.AppointmentViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientRecords : Page
    {
        private AllPatientsViewModel viewModel;
        public Patient Patient { get; set; }
        private User User { get; set; }
        public PatientRecords()
        {
            this.InitializeComponent();
            this.viewModel = new AllPatientsViewModel();
            this.DataContext = this.viewModel;
            this.setDateText();
            this.pInfo.DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ring.IsActive = true;
            this.User = (User)Application.Current.Resources[Constants.Nurse];
            this.Patient = (Patient) e.Parameter;
            await this.viewModel.LoadPatientsAppointments(this.Patient);
            await this.viewModel.LoadPatientCheckups(this.Patient);
            await this.viewModel.LoadPatientLabTest(this.Patient);
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            this.txtId.Text = this.User.Id;
            this.ring.IsActive = false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.User.LoggedIn = false;
            Application.Current.Resources.Remove(Constants.Nurse);
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void setDateText()
        {
            this.txtDate.Text = DateTime.Now.ToLongDateString();
        }

        private async void updatePatientInfo_Button_Click(object sender, RoutedEventArgs e)
        {
            var upcd = new UpdatePatientInfoContentDialog(this.Patient);
            await upcd.ShowAsync();
        }

        private void makeAppointment_Button_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(NewAppointmentPage), this.Patient);
        }

        private void enterCheckUp_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckupPage), this.Patient);
        }

        private async void orderLabTest_Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = new LabTestViewModel();
            await vm.LoadAllLabTestss();
            var oltcd = new OrderLabTestContentDialog(this.Patient)
            {
                AllTest = vm.AllTests
            };
            await oltcd.ShowAsync();
        }

        private async void labTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            var labTest = AllPatientsViewModel.SelectedLabTest;
            var pltr = new PatientLabTestResults(labTest);
            await pltr.ShowAsync();
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
            switch (result)
            {
                case ContentDialogResult.Secondary:
                    if (dialog.ChangesMade)
                    {
                        this.ring.IsActive = true;
                        await this.viewModel.LoadPatientsAppointments(this.Patient);
                        this.ring.IsActive = false;
                    }

                    break;
            }
        }

        private async void viewCheckup(object sender, ItemClickEventArgs e)
        {
            var checkup = e.ClickedItem as RoutineCheck;
            var cvm = new RoutineCheckupViewModel();
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

            switch (result)
            {
                case ContentDialogResult.Secondary:
                    if (dialog.ChangesMade)
                    {
                        this.ring.IsActive = true;
                        await this.viewModel.LoadPatientCheckups(this.Patient);
                        var mess = new MessageDialog("Your changes have been stored");
                        this.ring.IsActive = false;
                        await mess.ShowAsync();
                    }

                    break;
            }
        }
    }
}
