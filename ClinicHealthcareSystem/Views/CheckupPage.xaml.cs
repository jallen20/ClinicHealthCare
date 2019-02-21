using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckupPage : Page
    {
        #region Data members

        private User User;
        private readonly RoutineCheckupViewModel viewModel;

        #endregion

        #region Constructors

        public CheckupPage()
        {
            this.InitializeComponent();
            this.viewModel = new RoutineCheckupViewModel();
            DataContext = this.viewModel;
            this.setDateText();
        }

        #endregion

        #region Methods

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void setDateText()
        {
            this.txtDate.Text = DateTime.Now.ToLongDateString();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ring.IsActive = true;
            this.User = (User) Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            this.txtId.Text = this.User.Id;
            await this.viewModel.LoadAllAppointments();
            await this.viewModel.LoadAllNurses();
            await this.viewModel.LoadAllCheckups();
            this.ring.IsActive = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.User.LoggedIn = false;
            Application.Current.Resources.Remove(Constants.Nurse);
            Frame.Navigate(typeof(MainPage), null);
        }

        private void findNurse(object sender, RoutedEventArgs e)
        {
            //var list = this.viewModel.FindNurseByName(this.nFnameSearchBox.Text, this.nLnameSearchBox.Text);
            //this.nListview.ItemsSource = list.ToObservableCollection();
        }

        private async void addCheckup(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            try
            {
                var aId = int.Parse(this.aID.Text);
                var nId = int.Parse(this.nID.Text);
                var time = this.timeicker.Time;
                var date = this.datePicker.Date.Date.AddHours(time.Hours).AddMinutes(time.Minutes);
                var sys = int.Parse(this.sysReadind.Text);
                var bPr = int.Parse(this.bPressure.Text);
                var _weight = double.Parse(this.weight.Text);
                var temp = double.Parse(this.temp.Text);
                var ini = this.summaryTextBox.Text;
                var appointmentdal = new AppointmentDAL();
                var appointment = await appointmentdal.GetAppointmentById(aId);
                var nursedal = new NurseDAL();
                var nurse = await nursedal.GetNurseByNurseId(nId);
                var checkup = new RoutineCheck(appointment, nurse, appointment.Patient, date, sys, bPr, _weight, temp,
                    ini);
                await this.viewModel.AddCheckup(checkup);
                await this.viewModel.LoadAllAppointments();
                var confirmBox = new MessageDialog($"CheckUp completed  {date.ToShortDateString()}");
                this.ring.IsActive = false;
                await confirmBox.ShowAsync();
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        private void NFnameSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.FindNurseByName(this.nFnameSearchBox.Text, this.nLnameSearchBox.Text);
        }

        #endregion

        private async void viewDetails(object sender, ItemClickEventArgs e)
        {
            var checkup = e.ClickedItem as RoutineCheck;
            var dialog = new CheckupDetailedView() {
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
                        await this.viewModel.LoadAllCheckups();
                        var mess = new MessageDialog("Your changes have been stored");
                        this.ring.IsActive = false;
                        await mess.ShowAsync();
                    }

                    break;
            }
        }
    }
}