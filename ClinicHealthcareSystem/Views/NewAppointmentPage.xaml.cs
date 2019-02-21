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
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewAppointmentPage : Page
    {

        private AppointmentViewModel aViewModel;

        public User User { get; private set; }

        public Patient Patient { get; private set; }

        public NewAppointmentPage()
        {
            this.InitializeComponent();
            this.setDateText();

            this.aViewModel = new AppointmentViewModel();
            this.DataContext = this.aViewModel;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
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
            this.User = (User)Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            this.txtId.Text = this.User.Id;
            await this.aViewModel.LoadAllPatients();
            await this.aViewModel.LoadAllDoctors();
            await this.aViewModel.LoadAllAppointments();
            this.ring.IsActive = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.User.LoggedIn = false;
            Application.Current.Resources.Remove(Constants.Nurse);
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private async void findPatient(object sender, RoutedEventArgs e)
        {
            // var fname = !string.IsNullOrWhiteSpace(this.pFnameSearchBox.Text) ? this.pFnameSearchBox.Text : "0";
            // var lname = !string.IsNullOrWhiteSpace(this.pLnameSearchBox.Text) ? this.pLnameSearchBox.Text : "0";
            //// this.listview.ItemsSource = this.aViewModel.Search(fname, lname).ToObservableCollection();
        }

        private async void findDoctor(object sender, RoutedEventArgs e)
        {
            var fname = !string.IsNullOrWhiteSpace(this.docFnameSearchBox.Text) ? this.docFnameSearchBox.Text : "0";
            var lname = !string.IsNullOrWhiteSpace(this.docLnameSearchBox.Text) ? this.docLnameSearchBox.Text : "0";
            var ds = await this.aViewModel.GetDoctorsByName(fname, lname);
            this.docListview.ItemsSource = ds.ToObservableCollection();
        }

        private async void add(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            Doctor doc = null;
            try
            {
                var pId = int.Parse(this.pID.Text);
                var dId = int.Parse(this.dID.Text);
                var date = this.datePicker.Date.Date;
                var time = this.timeicker.Time;
                var appDate = date.AddHours(time.Hours).AddMinutes(time.Minutes).AddSeconds(time.Seconds);
                var description = this.summaryTextBox.Text;
                doc = await this.aViewModel.GetDoctor(dId);
                var pat = await this.aViewModel.GetPatient(pId);
                var appointment = new Appointment(pat, doc, appDate, description);
                await this.aViewModel.AddAppointment(appointment);
                await this.aViewModel.LoadAllAppointments();
                var confirmBox = new MessageDialog($"Appointment Added for {pat.FullName} with Dr.{doc.FullName} on {appDate.ToShortDateString()} {appDate.ToShortTimeString()}");
                this.ring.IsActive = false;
                await confirmBox.ShowAsync();
                return;
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                return;
            }

        }

        private async void filter(object sender, SelectionChangedEventArgs e)
        {
            this.ring.IsActive = true;
            await this.aViewModel.Filter();
            this.ring.IsActive = false;
        }

        private async void search(object sender, TextChangedEventArgs e)
        {
            this.ring.IsActive = true;
            await this.aViewModel.Search(this.pFnameSearchBox.Text);
            this.ring.IsActive = false;
        }

        private async void viewDetails(object sender, ItemClickEventArgs e)
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
                        await this.aViewModel.LoadAllAppointments();
                        this.ring.IsActive = false;
                    }

                    break;
            }
           
        }
    }
}
