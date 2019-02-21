using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Collections.Generic.Dictionary<string, string>;

namespace ClinicHealthcareSystem.ViewModels
{
    public class LabTestViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LabTest> allTests;
        private LabTestDAL labTestDAL;
        private static string patientId;
        private static string patientIdV2;

        public static List<LabTest> LabTests { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<LabTest> AllTests
        {
            get => this.allTests;
            set { this.allTests = value; this.OnPropertyChanged(); }
        }

        public static string PatientId
        {
            get => patientId;
            set { patientId = value; }
        }

        public string PatientIdV2
        {
            get => PatientId;
            set { patientIdV2 = value; }
        }

        public RelayCommand OrderLabTestCommand { get; set; }

        public LabTestViewModel()
        {
            this.LoadData();
            this.LoadCommands();
        }

        private void LoadData()
        {
            this.labTestDAL = new LabTestDAL();
           
            
            LabTests = new List<LabTest>();
        }

        public async Task LoadAllLabTestss()
        {
            var tests = await this.labTestDAL.GetAllLabTests();
            this.AllTests = tests.ToObservableCollection();
        }

        private void LoadCommands()
        {
            this.OrderLabTestCommand = new RelayCommand(OrderLabTest, CanOrderLabTest);
        }

        private bool CanOrderLabTest(object SelectedItems)
        {
            return this.AllTests != null;
        }

        private void OrderLabTest(object SelectedItems)
        {
            var today = DateTime.Now.Date;
            this.labTestDAL.OrderLabTest(Convert.ToInt32(this.PatientIdV2), today, this.calculateDueDate(today));
            
            foreach (var item in LabTests)
            {
                this.labTestDAL.EnterTestResult(item.Code, item.Name, null);
            }
        }

        private DateTime calculateDueDate(DateTime startDate)
        {
            int numberOfDays = 5;
            var currentDate = startDate;
            for (int i = 0; i < numberOfDays;)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                i++;
            }
            var dueDate = currentDate;
            return dueDate;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
