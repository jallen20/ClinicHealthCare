using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class OrderLabTestContentDialog : ContentDialog
    {
        private LabTestViewModel vm;
        public ObservableCollection<LabTest> AllTest { get; set; }

        public OrderLabTestContentDialog()
        {
            this.InitializeComponent();
            this.vm = new LabTestViewModel();
            this.DataContext = this.vm;
            this.allTests_ListView.DataContext = this;
        }

        

        public OrderLabTestContentDialog(Patient patient)
        {
            this.InitializeComponent();
            this.vm = new LabTestViewModel();
            this.DataContext = this.vm;
            this.allTests_ListView.DataContext = this;
            LabTestViewModel.PatientId = Convert.ToString(patient.Id);
            this.patientId_TextBox.Text = LabTestViewModel.PatientId;
            this.patientName_TextBlock.Text = patient.FullName;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //LabTestViewModel.LabTests = (List<LabTest>)this.allTests_ListView.ItemsSource;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (LabTest item in e.RemovedItems)
            {
                item.IsSelected = false;
            }

            foreach (LabTest item in e.AddedItems)
            {
                item.IsSelected = true;
                LabTestViewModel.LabTests.Add(item);
            }
        }
    }
}
