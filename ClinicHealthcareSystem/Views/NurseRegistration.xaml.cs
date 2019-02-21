using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NurseRegistration : Page
    {

        public IList<string> Genders { get; set; }
        public NurseRegistration()
        {
            this.InitializeComponent();
            this.Genders = new List<string>() {
                "M", "F"
            };
        }

        private async void register_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            try
            {
                
                var dal = new AdminDAL();
                var uid = !string.IsNullOrWhiteSpace(this.uidBox.Text) ? this.uidBox.Text 
                    : throw new ArgumentException("The user id cannot be empty");
                var pword = !string.IsNullOrWhiteSpace(this.pword.Text)
                    ? this.pword.Text
                    : throw new ArgumentException("The password cannot be empty");
                var user = new User(uid, this.fname_TextBox.Text, this.lname_TextBox.Text, this.ssn_TextBox.Text,
                    this.dob_DatePicker.Date.Date, (string) this.sex_ComboBox.SelectedItem, this.address_TextBox.Text,
                    this.phone_TextBox.Text);
                await dal.CreateNurse(uid, pword, user);
              
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                this.errors.Visibility = Visibility.Visible;
                this.errors.Text = ex.Message;
                await Task.Delay(4000);
                this.errors.Text = string.Empty;
                return;
            }

            var mes = new MessageDialog($"Nurse added");
            this.ring.IsActive = false;
            await mes.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NurseHomePage), null);
        }
    }
}
