using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.ViewModels;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QueryPage : Page
    {

        private User User;
        private readonly AdminQueryViewModel viewModel;
        public QueryPage()
        {
            this.InitializeComponent();
            this.viewModel = new AdminQueryViewModel();
            this.DataContext = this.viewModel;
            this.setDateText();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.User = (User)Application.Current.Resources[Constants.Nurse];
            this.txtFirstName.Text = this.User.FirstName;
            this.txtLastName.Text = this.User.LastName;
            this.txtRole.Text = this.User.Role;
            if (this.User.Role.Equals("Administrator"))
                this.btnBack.Visibility = Visibility.Visible;
            this.txtId.Text = this.User.Id;
        }

        private void setDateText()
        {
            this.txtDate.Text = DateTime.Now.ToLongDateString();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.User.LoggedIn = false;
            Application.Current.Resources.Remove(Constants.Nurse);
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private async void runQuery(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            try
            {
                var qry = this.queryBox.Text;
                if (qry.Contains("select", StringComparison.CurrentCultureIgnoreCase))
                {
                    await this.viewModel.RunQuery(this.queryBox.Text, this.grid);
                }
                else
                {
                    await this.viewModel.RunNonQuery(qry);
                    var mes = new MessageDialog("Your non-query has been executed");
                    this.ring.IsActive = false;
                    await mes.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog(ex.Message);
                this.ring.IsActive = false;
                await message.ShowAsync();
            }

            this.ring.IsActive = false;
        }
    }
}
