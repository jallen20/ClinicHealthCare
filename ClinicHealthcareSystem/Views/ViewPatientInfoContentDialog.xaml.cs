using ClinicHealthcareSystem.Models;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    public sealed partial class ViewPatientInfoContentDialog : ContentDialog
    {
        public string FName { get; set; }

        public string LName { get; set; }

        public string SSN { get; set; }

        public DateTime DOB { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public string Phone { get; set; }

        public ViewPatientInfoContentDialog()
        {
            this.InitializeComponent();
            //this.vm
            //this.fname_TextBlock.Text = this.patient.FirstName;
            //this.lname_TextBlock.Text = this.patient.LastName;
            //this.ssn_TextBlock.Text = this.patient.SSN;
            //this.dob_TextBlock.Text = this.patient.DOB.Date.ToString();
            //this.sex_TextBlock.Text = this.patient.Sex;
            //this.address_TextBlock.Text = this.patient.Address;
            //this.city_TextBlock.Text = this.patient.City;
            //this.state_TextBlock.Text = this.patient.State;
            //this.zip_TextBlock.Text = this.patient.Zip.ToString();
            //this.phone_TextBlock.Text = this.patient.Phone;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
