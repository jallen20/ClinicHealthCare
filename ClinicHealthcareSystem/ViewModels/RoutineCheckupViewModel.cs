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
    public class RoutineCheckupViewModel : INotifyPropertyChanged
    {
        #region Data members

        private Nurse selectedNurse;
        private Appointment selectedAppointment;
        private ObservableCollection<Nurse> nurses;
        private ObservableCollection<Appointment> appointments;

        private readonly RoutineCheckupDAL routineCheckupDal;
        private readonly NurseDAL nurseDal;
        private readonly AppointmentDAL appointmentDal;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected nurse.
        /// </summary>
        /// <value>
        /// The selected nurse.
        /// </value>
        public Nurse SelectedNurse
        {
            get => this.selectedNurse;
            set
            {
                this.selectedNurse = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected appointment.
        /// </summary>
        /// <value>
        /// The selected appointment.
        /// </value>
        public Appointment SelectedAppointment
        {
            get => this.selectedAppointment;
            set
            {
                this.selectedAppointment = value;
                this.OnPropertyChanged();
            }
        }

        private ObservableCollection<RoutineCheck> checkups;
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

        /// <summary>
        ///     Gets or sets the nurses.
        /// </summary>
        /// <value>
        ///     The nurses.
        /// </value>
        public ObservableCollection<Nurse> Nurses
        {
            get => this.nurses;
            set
            {
                this.nurses = value;
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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineCheckupViewModel"/> class.
        /// </summary>
        public RoutineCheckupViewModel()
        {
            this.routineCheckupDal = new RoutineCheckupDAL();
            this.appointmentDal = new AppointmentDAL();
            this.nurseDal = new NurseDAL();
        }

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Loads all appointments.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllAppointments()
        {
            var apps = await this.appointmentDal.GetAllInCompleteAppointments();
            this.Appointments = apps.ToObservableCollection();
        }

        /// <summary>
        ///     Loads all nurses.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllNurses()
        {
            var apps = await this.nurseDal.GetAllNurses();
            this.Nurses = apps.ToObservableCollection();
        }

        /// <summary>
        /// Loads all checkups.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllCheckups()
        {
            var checks = await this.routineCheckupDal.GetAllCheckups();
            this.Checkups = checks.ToObservableCollection();
        }


        /// <summary>
        /// Finds the name of the nurse by.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        public void FindNurseByName(string firstname, string lastname)
        {
            var newNurses = this.Nurses.Where(n => n.FirstName.Contains(firstname, StringComparison.CurrentCultureIgnoreCase) 
                                                   || n.LastName.Contains(lastname, StringComparison.CurrentCultureIgnoreCase)).ToList();
            this.Nurses = newNurses.ToObservableCollection();
        }

        private bool isDateTime(string text)
        {
            try
            {
                DateTime.Parse(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Adds the checkup.
        /// </summary>
        /// <param name="checkup">The checkup.</param>
        /// <returns></returns>
        public async Task AddCheckup(RoutineCheck checkup)
        {
            await this.routineCheckupDal.AddCheckp(checkup);
        }

        /// <summary>
        /// Edits the checkup.
        /// </summary>
        /// <param name="checkup">The checkup.</param>
        /// <returns></returns>
        public async Task EditCheckup(RoutineCheck checkup)
        {
            await this.routineCheckupDal.UpdateCheckup(checkup);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}