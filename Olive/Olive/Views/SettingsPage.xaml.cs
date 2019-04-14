using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olive.AppSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
		}

        public async void LogOutClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Log Out", "Are you sure you wish to log out?", "Yes", "No");
            if (answer)
            {
                Settings.UserKey = null;

                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
        }
	}
}