using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class PatientAppointment
    {
        public int AppointmentID { get; set; }
        public string DoctorFullname { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
