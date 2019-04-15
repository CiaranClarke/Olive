using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Olive.AppSpecific;
using Olive.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyItems : ContentPage
	{
        int pNo = SellItemPage.pNo;
        FirebaseAuth auth;
        List<string> prodNoImages = new List<string>();
        List<tblProducts> productList = new List<tblProducts>();
        List<tblProductImages> imageList = new List<tblProductImages>();
        private List<FirebaseObject<tblProductImages>> productImages;

        public MyItems ()
		{
            
			InitializeComponent ();

            myItems();
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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

            var products = dbData.Where(a => a.Object.prodSellerNo == Settings.UserKey).ToList();

            foreach (var e in products)
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

            foreach(var b in prodNoImages)
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

        public async void myItems()
        {
            //using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
            //{
            //    var product = db.Get<tblProducts>(pNo);



            //byte[] imageByte = Convert.FromBase64String(product.prodImageString);
            //ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(imageByte));
            //Image im = new Image();
            //im.HeightRequest = 250;
            //im.WidthRequest = 250;
            //im.Source = imgSrc;

            //imagesStack.Children.Add(im);
            //}
            try
            {
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
                    ep.Text = prodItem.prodDescription.ToString();
                    itemsStack.Children.Add(ep);
                }     
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "An error has occured", "OK");
            }

        }
	}
}