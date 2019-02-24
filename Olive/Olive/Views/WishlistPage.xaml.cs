using Olive.ViewModels;
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
	public partial class WishlistPage : ContentPage
	{
		public WishlistPage ()
		{
			InitializeComponent ();

            PopulateList();
            BindingContext = new WishlistViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (WishlistViewModel.hasWishlist == false)
            {
                NoCasesLabel.IsVisible = true;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as ItemProduct;
            await Navigation.PushAsync(new ViewItemPage(content.Id));
        }

        public void PopulateList()
        {
            if (WishlistViewModel.CreateWishlist() != null)
            {
                Wishlist.IsVisible = true;
                NoCasesLabel.IsVisible = false;

                //for (var i = 0; i < incidentsList.Count; i++)
                //{

                //}
            }
            else
            {
                Wishlist.IsVisible = false;
                NoCasesLabel.IsVisible = true;
            }
        }
    }
}