using ClinicHealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ClinicHealthcareSystem.Validators;
using ClinicHealthcareSystem.Views.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClinicHealthcareSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {

            this.InitializeComponent();

        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.ring.IsActive = true;
            try
            {
                var id = this.txtUserName.Text.Trim();
                var pWord = this.txtPassWord.Password.Trim();
                var nurseValidator = id[0] is 'n' ? (ILoginValidator) new NurseLoginValidator(id, pWord) : new AdminLoginValidator(id, pWord);
                var valid = await nurseValidator.IsValid();
                if (valid)
                {
                    if (id[0] == 'n')
                    {
                        var user = (Nurse) await nurseValidator.GetUser();
                        Application.Current.Resources.Add(Constants.Nurse, user);
                        this.Frame.Navigate(typeof(NurseHomePage), null, new EntranceNavigationTransitionInfo());
                    }
                    else
                    {
                        var user = (Administrator)await nurseValidator.GetUser();
                        Application.Current.Resources.Add(Constants.Nurse, user);
                        this.Frame.Navigate(typeof(AdminTransitionPage), null, new EntranceNavigationTransitionInfo());
                    }

                    this.ring.IsActive = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ring.IsActive = false;
                this.invalidCredintials.Text = ex.Message;
                this.invalidCredintials.Opacity = 1;
                await Task.Delay(5000);
                this.invalidCredintials.Opacity = 0;
            }
            

        }
    }


}
