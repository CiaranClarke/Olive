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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            txt_Email.Completed += (s, e) => txt_Password.Focus();
            txt_Password.Completed += (s, e) => SignInClicked(null, null);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            txt_Email.Text = "";
            txt_Password.Text = "";
        }

        public async void OnPasswordRecoveryTapped(object sender, EventArgs e)
        {
            //var signUpPage = new PasswordRecovery();
            //await Navigation.PushAsync(signUpPage, false);
        }

        private async void RegisterClicked(object sender, EventArgs e)
        {
            var signUpPage = new Views.SignUpPage();
            await Navigation.PushAsync(signUpPage, false);
        }

        public async void SignInClicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_Email.Text) & !String.IsNullOrEmpty(txt_Password.Text))
            {
                var main = new MainPage();
                await Navigation.PushAsync(main, false);
            }
            else
            {
                await DisplayAlert("Login", "Email address or password are not valid, please try again.", "Ok");
            }
        }
    }
}