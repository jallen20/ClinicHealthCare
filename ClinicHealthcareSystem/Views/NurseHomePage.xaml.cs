using ClinicHealthcareSystem.Models;
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
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NurseHomePage : Page
    {
        public User User { get; private set; }
        public NurseHomePage()
        {
            this.InitializeComponent();
            this.setDateText();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.User = (User)Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            if (this.User.Role.Equals("Administrator"))
            {
                this.btnBack.Visibility = Visibility.Visible;
                this.reg.Text = "Add Nurse";
                this.seac.Text = "Search";
                this.hideControls();
            }

            this.txtId.Text = this.User.Id;
        }

        private void hideControls()
        {
            this.btnCheckUp.Visibility = this.btnLab.Visibility = this.btnScheduleVist.Visibility =
                this.btnLab.Visibility = this.btnViewLabResulta.Visibility =
                    Visibility.Collapsed;
        }

        private void setDateText()
        {
            this.txtDate.Text = DateTime.Now.ToLongDateString();
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

        private void btnSearchPatient_Click(object sender, RoutedEventArgs e)
        {
            if (this.User.Role.Equals("Nurse"))
                this.Frame.Navigate(typeof(Views.SearchPatientPage), null);
            else
                this.Frame.Navigate(typeof(AdminSearchPage), null);
        }

        private void registerPatient_Button(object sender, RoutedEventArgs e)
        {
            if (this.User.Role.Equals("Nurse"))
                this.Frame.Navigate(typeof(Views.PatientRegistration), null);
            else
                this.Frame.Navigate(typeof(Views.NurseRegistration), null);
        }

        private void btnScheduleVist_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.NewAppointmentPage), null);
        }

        private void goToCheckUps(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.CheckupPage), null);
        }

        private async void btnLab_Click(object sender, RoutedEventArgs e)
        {
            var vm = new LabTestViewModel();
            await vm.LoadAllLabTestss();
            var oltcd = new OrderLabTestContentDialog()
            {
                AllTest = vm.AllTests
            };
            await oltcd.ShowAsync();
        }
    }
}
