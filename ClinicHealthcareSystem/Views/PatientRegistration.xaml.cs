using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientRegistration : Page
    {
        private PatientRegistrationController controller;

        public PatientRegistration()
        {
            this.InitializeComponent();
            controller = new PatientRegistrationController();
            var uSStates = new USStates();
            this.sex_ComboBox.ItemsSource = controller.Genders;
            this.sex_ComboBox.SelectedIndex = controller.SelectedIndex;
            this.state_ComboBox.ItemsSource = uSStates.States;
            this.state_ComboBox.SelectedIndex = 0;

            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ring.IsActive = true;
            var ps = await this.controller.LoadAllPatients();
            controller.Patients = ps.ToObservableCollection();
            this.ring.IsActive = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NurseHomePage));
        }

        private async void register_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            Patient patient = null;
            try
            {
                var fname = this.fname_TextBox.Text;
                var lname = this.lname_TextBox.Text;
                var ssn = this.ssn_TextBox.Text;

                var dob = this.dob_DatePicker.Date;
                var sex = this.sex_ComboBox?.SelectedValue;
                var address = this.address_TextBox.Text;
                var city = this.city_TextBox.Text;
                var state = this.state_ComboBox?.SelectedValue;
                var zip = !string.IsNullOrWhiteSpace(this.zip_TextBox.Text) ? int.Parse(this.zip_TextBox.Text) : throw new ArgumentException("The zip code must be filled");

                var phone = this.phone_TextBox.Text;
                patient = new Patient(fname, lname, ssn, dob.Date, sex as string, address, city, state as string,
                    zip, phone);

                await this.controller.AddPatient(patient);
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                this.errors.Text = ex.Message;
                this.errors.Visibility = Visibility.Visible;
                return;
            }

            this.ring.IsActive = false;
            var success =
                new MessageDialog(
                    $"Patient:{patient.FullName} registered on {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");
            await success.ShowAsync();
        }
    }
}
