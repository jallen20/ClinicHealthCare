using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;

namespace ClinicHealthcareSystem.ViewModels
{
    public class AppointmentViewModel : INotifyPropertyChanged
    {
        #region Data members

        private readonly PatientDAL patientDal;
        private readonly DoctorDAL doctorDal;
        private readonly AppointmentDAL appointmentDal;
        private Patient selectedPatient;

        private Doctor selectedDoctor;
        private ObservableCollection<Doctor> doctors;
        private ObservableCollection<Patient> patients;
        private ObservableCollection<Appointment> appointments;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the selected patient.
        /// </summary>
        /// <value>
        ///     The selected patient.
        /// </value>
        public Patient SelectedPatient
        {
            get => this.selectedPatient;
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the selected doctor.
        /// </summary>
        /// <value>
        ///     The selected doctor.
        /// </value>
        public Doctor SelectedDoctor
        {
            get => this.selectedDoctor;
            set
            {
                this.selectedDoctor = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public int SelectedItem { get; set; }

        /// <summary>
        ///     Gets or sets the doctors.
        /// </summary>
        /// <value>
        ///     The doctors.
        /// </value>
        public ObservableCollection<Doctor> Doctors
        {
            get => this.doctors;
            set
            {
                this.doctors = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the patients.
        /// </summary>
        /// <value>
        ///     The patients.
        /// </value>
        public ObservableCollection<Patient> Patients
        {
            get => this.patients;
            set
            {
                this.patients = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the appointments.
        /// </summary>
        /// <value>
        ///     The appointments.
        /// </value>
        public ObservableCollection<Appointment> Appointments
        {
            get => this.appointments;
            set
            {
                this.appointments = value;
                this.OnPropertyChanged();
            }
        }

        public List<int> ComboItems { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppointmentViewModel" /> class.
        /// </summary>
        public AppointmentViewModel()
        {
            this.doctorDal = new DoctorDAL();
            this.patientDal = new PatientDAL();
            this.appointmentDal = new AppointmentDAL();
            this.ComboItems = new List<int> { 0, 1, 2 };
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Loads all appointments.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllAppointments()
        {
            var apps = await this.appointmentDal.GetAllAppointments();
            this.Appointments = apps.ToObservableCollection();
        }

        /// <summary>
        ///     Loads all patients.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllPatients()
        {
            var pats = await this.patientDal.GetAllPatients();
            this.Patients = pats.ToObservableCollection();
        }

        /// <summary>
        ///     Loads all doctors.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllDoctors()
        {
            var pats = await this.doctorDal.GetAllDoctors();
            this.Doctors = pats.ToObservableCollection();
        }


        /// <summary>
        /// Searches the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public async Task Search(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                await this.LoadAllPatients();
                return;
            }

            var fnameLname = input?.Split(" ");
            if (!this.isInt(input))
            {
                if (fnameLname?.Length > 1)
                {
                    var spatients = this.Patients.Where(p =>
                        p.FirstName.Contains(fnameLname[0],
                            StringComparison.CurrentCultureIgnoreCase) ||
                        p.FirstName.Contains(fnameLname[1],
                            StringComparison.CurrentCultureIgnoreCase) ||
                        p.LastName.Contains(fnameLname[0],
                            StringComparison.CurrentCultureIgnoreCase) ||
                        p.LastName.Contains(fnameLname[1],
                            StringComparison.CurrentCultureIgnoreCase)).ToList();
                    this.Patients = spatients.ToObservableCollection();

                }
                else
                {
                    var spatients = this.Patients.Where(p =>
                                            p.FirstName.Contains(input,
                                                StringComparison.CurrentCultureIgnoreCase) ||
                                            p.LastName.Contains(input, StringComparison.CurrentCultureIgnoreCase))
                                        .ToList();
                    this.Patients = spatients.ToObservableCollection();
                }
            }
            else
            {
                var spatients = this.Patients
                                    .Where(p => p.SSN.StartsWith(input, StringComparison.CurrentCultureIgnoreCase))
                                    .ToList();
                this.Patients = spatients.ToObservableCollection();
            }

            if (this.isInt(input))
            {
                var spatients = this.Patients.Where(p =>
                    p.DOB.ToShortDateString().StartsWith(input,
                        StringComparison.CurrentCultureIgnoreCase)).ToList();
                this.Patients = spatients.ToObservableCollection();
                return;
            }

        }

        private bool isInt(string input)
        {
            try
            {
                Int16.Parse(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets the name of the doctors by.
        /// </summary>
        /// <param name="fname">The fname.</param>
        /// <param name="lname">The lname.</param>
        /// <returns></returns>
        public async Task<IList<Doctor>> GetDoctorsByName(string fname, string lname)
        {
            var doctors = await this.doctorDal.GetDoctorsByName(fname, lname);
            return doctors;
        }

        /// <summary>
        ///     Gets the doctor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Doctor> GetDoctor(int id)
        {
            return await this.doctorDal.GetDoctorByDoctorId(id);
        }

        /// <summary>
        ///     Gets the patient.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Patient> GetPatient(int id)
        {
            return await this.patientDal.GetPatientById(id);
        }

        /// <summary>
        ///     Adds the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        public async Task AddAppointment(Appointment appointment)
        {
            await this.appointmentDal.AddAppointment(appointment);
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            await this.appointmentDal.EditAppointment(appointment);
        }



        /// <summary>
        /// Filters the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task Filter()
        {
            switch (this.SelectedItem)
            {
                case 0:
                    var apps = await this.appointmentDal.GetAllAppointments();
                    this.Appointments = apps.ToObservableCollection();
                    break;
                case 1:
                    var apps2 = await this.appointmentDal.GetAllCompleteAppointments();
                    this.Appointments = apps2.ToObservableCollection();
                    break;
                case 2:
                    var apps3 = await this.appointmentDal.GetAllInCompleteAppointments();
                    this.Appointments = apps3.ToObservableCollection();
                    break;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}