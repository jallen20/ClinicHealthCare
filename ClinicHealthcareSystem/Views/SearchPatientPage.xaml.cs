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
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Views.Utility;
using ClinicHealthcareSystem.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPatientPage : Page
    {
        public Nurse Nurse { get; private set; }
        private AllPatientsViewModel viewModel;

        public SearchPatientPage()
        {
            this.InitializeComponent();
            this.setDateText();
            this.viewModel = new AllPatientsViewModel();
            this.DataContext = this.viewModel;
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
            this.Nurse = (Nurse)Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.Nurse.FirstName;
            this.txtLastName.Text = this.Nurse.LastName;
            this.txtRole.Text = "Nurse";
            this.txtId.Text = this.Nurse.Id;
            await this.viewModel.LoadAllPatients();
            this.ring.IsActive = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Nurse.LoggedIn = false;
            Application.Current.Resources.Remove(Constants.Nurse);
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private async void edit(object sender, RoutedEventArgs routedEventArgs)
        {
            var patient = this.listview.SelectedItem as Patient;
            if (patient != null)
            {
                UpdatePatientInfoContentDialog upicd = new UpdatePatientInfoContentDialog(patient);
                var result = await upicd.ShowAsync();
                switch (result)
                {
                    case ContentDialogResult.Primary:
                        this.ring.IsActive = true;
                        await this.viewModel.LoadAllPatients();
                        this.ring.IsActive = false;
                        break;
                }
               
            }
        }

        private async void setPaitentAppointments(object sender, ItemClickEventArgs e)
        {
            this.ring.IsActive = true;
            var patient = e.ClickedItem as Patient;
            try
            {


                await this.viewModel.FindPatientAppointments(patient);
                await this.viewModel.FindPatientRoutineChecks(patient);
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                var mes = new MessageDialog(ex.Message);
                await mes.ShowAsync();
            }
            this.ring.IsActive = false;
        }

        private void search_Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientRecords),e.ClickedItem as Patient);
        }

        private void listView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //private bool isSearchValuesEmpty()
        //{
        //    var dob = this.dob_TextBox.Text;
        //    var fname = this.fname_TextBox.Text;
        //    var lname = this.lname_TextBox.Text;
        //    var allEmpty = (string.IsNullOrEmpty(dob) || string.IsNullOrWhiteSpace(dob))
        //        && (string.IsNullOrEmpty(fname) || string.IsNullOrWhiteSpace(fname))
        //        && (string.IsNullOrEmpty(lname) || string.IsNullOrWhiteSpace(lname));
        //    return allEmpty;
        //}
    }
}
