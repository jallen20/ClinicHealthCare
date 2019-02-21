using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    public sealed partial class UpdatePatientInfoContentDialog : ContentDialog
    {
        private Patient patient;
        private EditPatientController controller;

        public UpdatePatientInfoContentDialog(Patient patient)
        {
            this.InitializeComponent();
            
            this.patient = patient;
            this.controller = new EditPatientController();
            var uSStates = new USStates();
            this.sex_ComboBox.ItemsSource = new List<string>
            {
                "Male",
                "Female"
            }.ToObservableCollection();
            this.state_ComboBox.ItemsSource = uSStates.States;

            this.errormessage_TextBlock.Text = "";
            this.ssn_TextBox.Text = this.patient.SSN;
            this.fname_TextBox.Text = this.patient.FirstName;
            this.lname_TextBox.Text = this.patient.LastName;
            this.dob_DatePicker.Date = this.patient.DOB;
            this.sex_ComboBox.SelectedItem = this.patient.Sex;
            this.phone_TextBox.Text = this.patient.Phone;
            this.address_TextBox.Text = this.patient.Address;
            this.city_TextBox.Text = this.patient.City;
            this.state_ComboBox.SelectedItem = this.patient.State;
            this.zip_TextBox.Text = this.patient.Zip.ToString();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //VALIDATION?
            //BUTTON NAMES?

            var fname = this.fname_TextBox.Text;
            var lname = this.lname_TextBox.Text;
            var ssn = this.ssn_TextBox.Text;
            var ssnformat = Regex.Match(ssn, @"\+?[0-9]{9}");
            var dob = this.dob_DatePicker.Date;
            var sex = this.sex_ComboBox.SelectedItem.ToString();
            var address = this.address_TextBox.Text;
            var city = this.city_TextBox.Text;
            var state = this.state_ComboBox.SelectedItem.ToString();
            var zip = this.zip_TextBox.Text;
            var zipformat = Regex.Match(zip, @"\+?[0-9]{5}");
            var phone = this.phone_TextBox.Text;
            var phoneformat = Regex.Match(phone, @"\+?[0-9]{10}");

            if (!(fname.Length > 0 && fname.Length < 50) || string.IsNullOrEmpty(fname))
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid First Name.**";
                args.Cancel = false;
            }

            else if (!(lname.Length > 0 && lname.Length < 50) || string.IsNullOrEmpty(lname))
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid Last Name.**";
                //return;
            }

            else if (!ssnformat.Success)
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid SSN (Numbers Only).**";
                //return;
            }

            else if (dob.Date > DateTime.Now || dob.Date == null)
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please select a valid Date of Birth.**";
                //return;
            }

            else if (!(sex.Equals("Male") || sex.Equals("Female")))
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please select a valid gender.**";
                //return;
            }

            else if (!phoneformat.Success)
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid Phone Number (Numbers Only).**";
                //return;
            }

            else if (!(address.Length > 0 && address.Length < 100) || string.IsNullOrEmpty(address))
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid Address.**";
                //return;
            }

            else if (!(city.Length > 0 && city.Length < 100) || string.IsNullOrEmpty(city))
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid City.**";
                //return;
            }

            else if (!zipformat.Success)
            {
                this.errormessage_TextBlock.Text = "**Invalid! Please enter a valid ZIP Code (Numbers Only).**";
                //return;
            }

            else
            {
                await this.controller.EditPatient(this.patient.Id, fname, lname, ssn, dob.Date, sex, address, city, state, Convert.ToInt32(zip), phone);
                return;
            }
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
