using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class PatientCheckUps
    {
        public int AppointmentId { get; set; }

        public string NurseFullname { get; set; }

        public DateTime Date { get; set; }

        public double SystolicReading { get; set; }

        public double DiastolicBloodPressure { get; set; }

        public double Weight { get; set; }

        public string InitialDiagnosis { get; set; }

        public double Temperature { get; set; }
    }
}
