using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }

        public DateTime Date { get; set; }

        public string ShorTime => this.Date.ToShortTimeString();

        public string ShortDate => this.Date.ToShortDateString();

        public string Description { get; set; }

        public Appointment(Patient p, Doctor d, DateTime date, string description)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date));
            }

            if (description.Length > 200)
            {
                throw new ArgumentException("The description cannot exceed 200 charectors");
            }

            this.Patient = p ?? throw new ArgumentNullException(nameof(p));
            this.Doctor = d ?? throw new ArgumentNullException(nameof(d));
            this.Date = date;
            this.Description = description ?? throw new ArgumentNullException(nameof(description));

        }
    }
}
