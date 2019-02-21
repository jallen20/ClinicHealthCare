using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class RoutineCheck
    {
        /// <summary>
        /// Gets or sets the appointment.
        /// </summary>
        /// <value>
        /// The appointment.
        /// </value>
        public Appointment Appointment { get; set; }

        /// <summary>
        /// Gets or sets the nurse.
        /// </summary>
        /// <value>
        /// The nurse.
        /// </value>
        public Nurse Nurse { get; set; }

        /// <summary>
        /// Gets or sets the patient.
        /// </summary>
        /// <value>
        /// The patient.
        /// </value>
        public Patient Patient { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets the short date.
        /// </summary>
        /// <value>
        /// The short date.
        /// </value>
        public string ShortDate => this.Date.ToShortDateString();

        /// <summary>
        /// Gets the short time.
        /// </summary>
        /// <value>
        /// The short time.
        /// </value>
        public string ShortTime => this.Date.ToShortTimeString();

        /// <summary>
        /// Gets or sets the systolic reading.
        /// </summary>
        /// <value>
        /// The systolic reading.
        /// </value>
        public double SystolicReading { get; set; }

        /// <summary>
        /// Gets or sets the diastolic blood pressure.
        /// </summary>
        /// <value>
        /// The diastolic blood pressure.
        /// </value>
        public double DiastolicBloodPressure { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the initial diagnosis.
        /// </summary>
        /// <value>
        /// The initial diagnosis.
        /// </value>
        public string InitialDiagnosis { get; set; }

        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public double Temperature { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineCheck"/> class.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="nurse">The nurse.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="date">The date.</param>
        /// <param name="systolicReading">The systolic reading.</param>
        /// <param name="diastolicBloodPressure">The diastolic blood pressure.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="temp">The temporary.</param>
        /// <param name="initialDiagn">The initial diagn.</param>
        /// <exception cref="ArgumentException">
        /// Systolic Reading less than 0
        /// or
        /// Diastolic Blood Pressure less than 0
        /// or
        /// Weight less than 0
        /// or
        /// temperature less than 0
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// appointment
        /// or
        /// nurse
        /// or
        /// patient
        /// or
        /// date
        /// or
        /// initialDiagn
        /// </exception>
        public RoutineCheck(Appointment appointment, Nurse nurse, Patient patient, DateTime? date, double systolicReading, double diastolicBloodPressure, double weight, double temp, string initialDiagn)
        {
            if (systolicReading < 0)
            {
                throw new ArgumentException("Systolic Reading less than 0");
            }

            if (diastolicBloodPressure < 0)
            {
                throw new ArgumentException("Diastolic Blood Pressure less than 0");
            }

            if (weight < 0)
            {
                throw new ArgumentException("Weight less than 0");
            }

            if (temp < 0)
            {
                throw new ArgumentException("temperature less than 0");
            }

            this.Appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
            this.Nurse = nurse ?? throw new ArgumentNullException(nameof(nurse));
            this.Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            this.Date = date ?? throw new ArgumentNullException(nameof(date));
            this.SystolicReading = systolicReading;
            this.DiastolicBloodPressure = diastolicBloodPressure;
            this.Weight = weight;
            this.InitialDiagnosis = initialDiagn ?? throw new ArgumentNullException(nameof(initialDiagn));
        }


    }
}
