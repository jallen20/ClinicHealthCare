using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.ViewModels
{
    public class PatientRegistrationController
    {
        private PatientDAL patientRepository;

        private ObservableCollection<Patient> patients;

        public ObservableCollection<Patient> Patients
        {
            get => this.patients;
            set
            {
                this.patients = value;
            }
        }

        public ObservableCollection<string> Genders { get; set; }

        private int selectedIndex;

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
            }
        }

        public PatientRegistrationController()
        {
            this.patientRepository = new PatientDAL();
            
            this.Genders = new List<string>
            {
                "Male",
                "Female"
            }.ToObservableCollection();
            this.SelectedIndex = 0;
        }

        public async Task<IList<Patient>> LoadAllPatients()
        {
            return await this.patientRepository.GetAllPatients();
        }

        public async Task AddPatient(Patient patient)
        {
            await this.patientRepository.AddPatientAsync(patient);
        }
    }
}
