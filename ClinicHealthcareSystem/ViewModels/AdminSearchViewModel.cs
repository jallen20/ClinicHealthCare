using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Extensions;
using ClinicHealthcareSystem.Models;

namespace ClinicHealthcareSystem.ViewModels
{
    public class AdminSearchViewModel : INotifyPropertyChanged
    {
        #region Data members

        private readonly AppointmentDAL appointmentDal;
        private readonly RoutineCheckupDAL routineCheckupDal;
        private ObservableCollection<Appointment> appointment;

        private ObservableCollection<RoutineCheck> checkups;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the appointments.
        /// </summary>
        /// <value>
        ///     The appointments.
        /// </value>
        public ObservableCollection<Appointment> Appointments
        {
            get => this.appointment;
            set
            {
                this.appointment = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the checkups.
        /// </summary>
        /// <value>
        ///     The checkups.
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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdminSearchViewModel" /> class.
        /// </summary>
        public AdminSearchViewModel()
        {
            this.appointmentDal = new AppointmentDAL();
            this.routineCheckupDal = new RoutineCheckupDAL();
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Loads the appointments.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAppointments()
        {
            var apps = await this.appointmentDal.GetAllAppointments();
            this.Appointments = apps.ToObservableCollection();
        }

        /// <summary>
        ///     Loads the checkups.
        /// </summary>
        /// <returns></returns>
        public async Task LoadCheckups()
        {
            var checks = await this.routineCheckupDal.GetAllCheckups();
            this.Checkups = checks.ToObservableCollection();
        }

        /// <summary>
        ///     Searches the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="list1">The list1.</param>
        /// <param name="list2">The list2.</param>
        /// <returns></returns>
        public async Task Search(DateTime start, DateTime end, ListView list1, ListView list2)
        {
            var aTask = await this.appointmentDal.GetAllAppointments();
            var alist = aTask.Where(a => a.Date.CompareTo(start) >= 0 && a.Date.CompareTo(end) <= 0)
                             .OrderBy(a => a.Date)
                             .ThenBy(a => a.Patient.LastName).ToList();

            var bTask = await this.routineCheckupDal.GetAllCheckups();
            var bList = bTask.Where(a => a.Date.CompareTo(start) >= 0 && a.Date.CompareTo(end) <= 0)
                             .OrderBy(a => a.Date)
                             .ThenBy(a => a.Patient.LastName).ToList();

            list1.ItemsSource = alist.ToObservableCollection();
            list2.ItemsSource = bList.ToObservableCollection();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}