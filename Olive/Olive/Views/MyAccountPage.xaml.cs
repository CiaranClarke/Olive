using Olive.AppSpecific;
using Olive.Interfaces;
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

        public async void MyItemsClicked(object sender, EventArgs e)
        {
            var myItemsPage = new Views.MyItems();
            //await Navigation.PushAsync(sellItemPage, false);
            await Navigation.PushModalAsync(myItemsPage, false);
        }

        public async void WishlistClicked(object sender, EventArgs e)
        {
            var wishlistPage = new Views.WishlistPage();
            await Navigation.PushModalAsync(wishlistPage, false);
        }
    }
}