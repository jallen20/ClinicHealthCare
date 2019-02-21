using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using ClinicHealthcareSystem.DAL;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace ClinicHealthcareSystem.ViewModels
{
    public class AdminQueryViewModel : INotifyPropertyChanged
    {
        #region Data members

        private ObservableCollection<object> qryResults;
        private readonly AdminDAL adminDal;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the query results.
        /// </summary>
        /// <value>
        ///     The query results.
        /// </value>
        public ObservableCollection<object> QueryResults
        {
            get => this.qryResults;
            set
            {
                this.qryResults = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdminQueryViewModel" /> class.
        /// </summary>
        public AdminQueryViewModel()
        {
            this.adminDal = new AdminDAL();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Runs the query.
        /// </summary>
        /// <param name="qry">The qry.</param>
        /// <param name="grid">The grid.</param>
        /// <returns></returns>
        public async Task RunQuery(string qry, DataGrid grid)
        {
            grid.Columns.Clear();
            var table = await this.adminDal.RunQuery(qry);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                grid.Columns.Add(new DataGridTextColumn {
                    Header = table.Columns[i].ColumnName,
                    Binding = new Binding {Path = new PropertyPath($"[{i}]")}
                });
            }

            var data = new ObservableCollection<object>();
            foreach (DataRow current in table.Rows)
            {
                data.Add(current.ItemArray);
            }

            this.QueryResults = data;
        }

        /// <summary>
        /// Runs the non query.
        /// </summary>
        /// <param name="qry">The qry.</param>
        /// <returns></returns>
        public async Task RunNonQuery(string qry)
        {
            await this.adminDal.RunNonQuery(qry);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}