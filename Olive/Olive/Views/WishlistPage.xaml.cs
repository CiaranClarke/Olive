using Firebase.Database;
using Newtonsoft.Json;
using Olive.AppSpecific;
using Olive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public static bool hasWishlist = true;

        List<string> prodNoImages = new List<string>();
        List<tblWishlist> wishlistNumbers = new List<tblWishlist>();
        List<tblProducts> productList = new List<tblProducts>();
        List<tblProductImages> imageList = new List<tblProductImages>();
        private List<FirebaseObject<tblProducts>> productsWishlist;
        private List<FirebaseObject<tblProductImages>> productImages;
        ObservableCollection<string> data = new ObservableCollection<string>();

        
        public WishlistPage ()
		{
			InitializeComponent ();

            CreateWishlist();
            //PopulateList();
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
            //await Navigation.PushAsync(new ViewItemPage(content.Id));
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public async Task<List<tblWishlist>> CreateWishlist(string email, string password)
        {
            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            //var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
              });

            var dbData = await db
                    .Child("Wishlist")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblWishlist>();

            var wishlist = dbData.Where(a => a.Object.userNo == Settings.UserKey).ToList();

            if(wishlist.Count != 0)
            {
                hasWishlist = true;

                foreach (var e in wishlist)
                {
                    var json = JsonConvert.SerializeObject(e.Object);
                    var wishObject = JsonConvert.DeserializeObject<tblWishlist>(json);
                    wishlistNumbers.Add(wishObject);
                }
            }
            else
            {
                hasWishlist = false;
            }
           
            return wishlistNumbers;
        }

        public async Task<List<tblProducts>> CreateItems(string email, string password)
        {
            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            //var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
              });

            var dbData = await db
                    .Child("Products")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblProducts>();

            foreach (var b in wishlistNumbers)
            {
                productsWishlist = dbData.Where(a => a.Key == b.productNo).ToList();
            }

            foreach (var e in productsWishlist)
            {
                e.Object.productNo = e.Key;
                var json = JsonConvert.SerializeObject(e.Object);
                var prodObject = JsonConvert.DeserializeObject<tblProducts>(json);
                prodNoImages.Add(e.Key);
                productList.Add(prodObject);
            }

            return productList;
        }

        public async Task<List<tblProductImages>> CreateProductImages(string email, string password)
        {
            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            //var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
             "https://olive-4a870.firebaseio.com/",
             new FirebaseOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
             });

            var dbData = await db
                    .Child("ProductImages")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblProductImages>();

            foreach (var b in prodNoImages)
            {
                productImages = dbData.Where(a => a.Object.productNo == b).ToList();
            }


            foreach (var e in productImages)
            {
                var json = JsonConvert.SerializeObject(e.Object);
                var prodImageObject = JsonConvert.DeserializeObject<tblProductImages>(json);
                imageList.Add(prodImageObject);
            }

            //prodNoForImages = some.Object.productNo;
            return imageList;
        }

        public async void CreateWishlist()
        {
            //var result = new List<tblProducts>();

            try
            {
                await CreateWishlist(Settings.email, Settings.password);
                if(hasWishlist == true)
                {
                    Wishlist.IsVisible = true;
                    NoCasesLabel.IsVisible = false;

                    await CreateItems(Settings.email, Settings.password);
                    await CreateProductImages(Settings.email, Settings.password);


                    var result = from f in productList
                                 join s in imageList on f.productNo equals s.productNo into g
                                 select new
                                 {
                                     f.productNo,
                                     f.prodCategory,
                                     f.prodSubCategory,
                                     f.prodPrice,
                                     f.prodDescription,
                                     f.prodSize,
                                     f.prodColour,
                                     f.prodBrand,
                                     f.prodLocation,
                                     f.prodSold,
                                     f.prodSellerNo,
                                     g.First().prodImageString
                                 };

                    foreach (var e in result)
                    {
                        byte[] imageByte = Convert.FromBase64String(e.prodImageString);
                        ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(imageByte));
                        Image im = new Image();
                        im.HeightRequest = 500;
                        im.WidthRequest = 400;
                        im.Source = imgSrc;

                        var prodItem = new tblProducts
                        {
                            prodCategory = e.prodCategory,
                            prodSubCategory = e.prodSubCategory,
                            prodPrice = e.prodPrice,
                            prodDescription = e.prodDescription,
                            prodSize = e.prodSize,
                            prodColour = e.prodColour,
                            prodBrand = e.prodBrand,
                            prodLocation = e.prodLocation,
                            prodSold = e.prodSold,
                            prodSellerNo = e.prodSellerNo
                        };

                        Label ep = new Label();
                        ep.FontSize = 20;
                        ep.HorizontalOptions = LayoutOptions.StartAndExpand;
                        ep.Text = prodItem.prodDescription.ToString();
                        Wishlist.Children.Add(ep);
                    }

                    //Wishlist.ItemsSource = data;
                }
                else
                {
                    Wishlist.IsVisible = false;
                    NoCasesLabel.IsVisible = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}