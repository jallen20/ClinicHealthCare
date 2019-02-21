using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.ViewModels
{
    public class EditPatientController
    {
        private PatientDAL patientRepository;

        public ObservableCollection<string> Genders { get; set; }

        public EditPatientController()
        {
            this.patientRepository = new PatientDAL();
        }

        public async Task EditPatient(int id, string fname, string lname, string ssn, DateTime dob, string sex, string address, string city, string state, int zip, string phone)
        {
           await this.patientRepository.EditPatientInfo(id, fname, lname, ssn, dob, sex, address, city, state, zip, phone);
        }
    }
}
