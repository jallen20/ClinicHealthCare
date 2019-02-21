using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.Util;
using ClinicHealthcareSystem.Views;

namespace ClinicHealthcareSystem.ViewModels
{
    public class AllPatientsViewModel : INotifyPropertyChanged
    {
        #region Data members

        private PatientDAL patientRepository;
        private AppointmentDAL appointmentRepository;
        private RoutineCheckupDAL checkupDalRepository;
        private LabTestDAL labTestDAL;
        private ObservableCollection<Patient> patients;
        private static Patient selectedPatient;
        private ObservableCollection<Appointment> appointments;
        private ObservableCollection<Appointment> patientAppointments;
        private ObservableCollection<RoutineCheck> patientCheckUps;
        private ObservableCollection<PatientLabTests> patientLabTest;
        private static PatientLabTests selectedLabTest;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the patients.
        /// </summary>
        /// <value>
        /// The patients.
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
        /// Gets or sets the selected patient.
        /// </summary>
        /// <value>
        /// The selected patient.
        /// </value>
        public static Patient SelectedPatient
        {
            get => selectedPatient;
            set
            {
                selectedPatient = value;
                //this.OnPropertyChanged();
            }
        }

        public Patient SelectedPatientV2 => SelectedPatient;

        /// <summary>
        /// Gets or sets the appointments.
        /// </summary>
        /// <value>
        /// The appointments.
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

        public ObservableCollection<Appointment> AppointmentsV2
        {
            get => this.patientAppointments;
            set { this.patientAppointments = value; this.OnPropertyChanged(); }
        }

        public async Task LoadPatientsAppointments(Patient patient)
        {
            var appnts = await this.appointmentRepository.GetAppointmentsByPatient(patient.Id);
            this.AppointmentsV2 = appnts.ToObservableCollection();
        }

        public async Task LoadPatientCheckups(Patient patient)
        {
            var checks = await this.checkupDalRepository.GetPatientRoutineChecks(patient);
            this.CheckUpsV2 = checks.ToObservableCollection();
        }
        public ObservableCollection<RoutineCheck> CheckUpsV2
        {
            get => this.patientCheckUps;
            set { this.patientCheckUps = value; this.OnPropertyChanged(); }
        }

        public ObservableCollection<PatientLabTests> LabTestsV2
        {
            get => this.patientLabTest;
            set { this.patientLabTest = value; this.OnPropertyChanged(); }
        }

        public async Task LoadPatientLabTest(Patient patient)
        {
            var labs = await this.labTestDAL.GetPatientLabTests(patient.SSN);
            this.LabTestsV2 = labs.ToObservableCollection();
        }

        public static PatientLabTests SelectedLabTest
        {
            get => selectedLabTest;
            set { selectedLabTest = value; }
        }

        /// <summary>
        /// Gets or sets the checkups.
        /// </summary>
        /// <value>
        /// The checkups.
        /// </value>
        public ObservableCollection<RoutineCheck> Checkups
        {
            get => this.checkups;
            set
            {
                this.checkups = value;
                this.OnPropertyChanged();
            }
        }
        private ObservableCollection<RoutineCheck> checkups;

        public string SearchDOBText { get; set; }
        public string SearchFnameText { get; set; }
        public string SearchLnameText { get; set; }

        public RelayCommand SearchPatientCommand { get; set; }
        public RelayCommand LoadAllPatientsCommand { get; set; }
        public RelayCommand GetPatientAppointmentsCommand { get; set; }
        public RelayCommand EditPatientInfoCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllPatientsViewModel"/> class.
        /// </summary>
        public AllPatientsViewModel()
        {
            this.LoadData();
            this.LoadCommands();
           }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadData()
        {
            this.patientRepository = new PatientDAL();
            this.appointmentRepository = new AppointmentDAL();
            this.checkupDalRepository = new RoutineCheckupDAL();
            this.labTestDAL = new LabTestDAL();
            this.Patients = new ObservableCollection<Patient>();
        }

        private void LoadCommands()
        {
            this.LoadAllPatientsCommand = new RelayCommand(LoadAllPatients, CanLoadAllPatients);
            this.SearchPatientCommand = new RelayCommand(SearchPatient, CanSearchPatient);
            this.GetPatientAppointmentsCommand = new RelayCommand(GetPatientAppointments, CanGetPatientsAppointments);
            this.EditPatientInfoCommand = new RelayCommand(EditPatientInfo, CanEditPatientInfo);
        }

        private bool CanEditPatientInfo(object obj)
        {
            return this.Patients != null;
        }

        private async void EditPatientInfo(object obj)
        {
            await this.patientRepository.EditPatientInfo(this.SelectedPatientV2.Id, this.SelectedPatientV2.FirstName, this.SelectedPatientV2.LastName,
                this.SelectedPatientV2.SSN, this.SelectedPatientV2.DOB, this.SelectedPatientV2.Sex, this.SelectedPatientV2.Address,
                this.SelectedPatientV2.City, this.SelectedPatientV2.State, this.SelectedPatientV2.Zip, this.SelectedPatientV2.Phone);
        }

        private bool CanGetPatientsAppointments(object obj)
        {
            return this.Patients != null;
        }

        private void GetPatientAppointments(object obj)
        {
            //var apps = this.appointmentRepository.GetAppointmentsByPatient(this.SelectedPatient.Id);
            //this.Appointments = apps.Result.ToObservableCollection();
            //return this.Appointments;
        }

        private bool CanLoadAllPatients(object obj)
        {
            return this.Patients != null;
        }

        private async void LoadAllPatients(object obj)
        {
            var ps = await this.patientRepository.GetAllPatients();
            this.Patients = ps.ToObservableCollection();
        }

        private bool CanSearchPatient(object obj)
        {
            return this.Patients != null;
        }

        private async void SearchPatient(object obj)
        {
            var ps = await this.patientRepository.SearchPatient(SearchDOBText, SearchFnameText, SearchLnameText);
            this.Patients = ps.ToObservableCollection();
        }

        /// <summary>
        /// Loads all patients.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllPatients()
        {
            var ps = await this.patientRepository.GetAllPatients();
            this.Patients = ps.ToObservableCollection();
        }

        /// <summary>
        /// Finds the patient appointments.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public async Task FindPatientAppointments(Patient patient)
        {
            var apps = await this.appointmentRepository.GetAppointmentsByPatient(patient.Id);
            this.Appointments = apps.ToObservableCollection();
        }

        /// <summary>
        /// Finds the patient routine checks.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public async Task FindPatientRoutineChecks(Patient patient)
        {
           var checks = await this.checkupDalRepository.GetPatientRoutineChecks(patient);
            this.Checkups = checks.ToObservableCollection();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}