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
    public sealed partial class PatientLabTestResults : ContentDialog
    {
        //public PatientLabTestResults()
        //{
        //    this.InitializeComponent();
        //}

        public PatientLabTestResults(PatientLabTests labTest)
        {
            this.InitializeComponent();

            //this.code_TextBlock.Text = labTest.Code;
            //this.name_TextBlock.Text = labTest.Name;
            //this.result_TextBox.Text = labTest.Result;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
