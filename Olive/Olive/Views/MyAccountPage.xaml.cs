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
	public partial class MyAccountPage : ContentPage
	{
		public MyAccountPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
		}

        public async void SellItemClicked(object sender, EventArgs e)
        {
            var sellItemPage = new Views.SellItemPage();
            //await Navigation.PushAsync(sellItemPage, false);
            await Navigation.PushModalAsync(sellItemPage, false);
        }

    }
}