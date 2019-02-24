using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{
		public ForgotPasswordPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, true);
        }

        private async void ResetPasswordClicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var LoginPage = new LoginPage();
                    await Navigation.PushAsync(LoginPage, false);

                }
                else
                {
                    await DisplayAlert("Connection Issue", "Please check your network/internet connection, you must be connected to the internet to reset your password.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Retry");
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}