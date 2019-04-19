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
	public partial class ViewItemPage : ContentPage
	{
        public static string productID;
        public static ViewItemPage ViewItemPageCS;
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        private tblProducts productDetails;
        private List<FirebaseObject<tblProductImages>> productImages;
        List<tblProductImages> imageList = new List<tblProductImages>();

        public ViewItemPage (string productId)
        {
            ViewItemPageCS = this;
            InitializeComponent();

            productID = productId;

            CreateItem();
        }

        public async Task<tblProducts> CreateItem(string email, string password)
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

            var products = dbData.Where(a => a.Key == productID).ToList();

            foreach (var e in products)
            {
                e.Object.productNo = e.Key;
                var json = JsonConvert.SerializeObject(e.Object);
                var prodObject = JsonConvert.DeserializeObject<tblProducts>(json);
                productDetails = prodObject;
            }

            return productDetails;
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

            
            productImages = dbData.Where(a => a.Object.productNo == productID).ToList();


            foreach (var e in productImages)
            {
                var json = JsonConvert.SerializeObject(e.Object);
                var prodImageObject = JsonConvert.DeserializeObject<tblProductImages>(json);
                imageList.Add(prodImageObject);
            }

            //prodNoForImages = some.Object.productNo;
            return imageList;
        }

        public async void CreateItem()
        {
            try
            {
                await CreateItem(Settings.email, Settings.password);
                await CreateProductImages(Settings.email, Settings.password);


                foreach (var e in imageList)
                {
                    byte[] imageByte = Convert.FromBase64String(e.prodImageString);
                    ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(imageByte));
                    Image im = new Image();
                    im.HeightRequest = 500;
                    im.WidthRequest = 400;
                    im.Source = imgSrc;

                    //imageSlide = im;
                    //imageSources.Add(im);
                    imagestack.Children.Add(im);
                }

                lbl_Title.Text = productDetails.prodDescription;
                lbl_Price.Text = productDetails.prodPrice.ToString();
                lbl_Size.Text = productDetails.prodSize;
                lbl_Category.Text = productDetails.prodCategory;
                lbl_SubCategory.Text = productDetails.prodSubCategory;
                lbl_Brand.Text = productDetails.prodBrand;
                lbl_Colour.Text = productDetails.prodColour;
                lbl_Location.Text = productDetails.prodLocation;


                //imgSlider.Images = imageSources;
            }
            catch(Exception)
            {

            }
        }

        public async void buyitem_clicked(object sender, EventArgs e)
        {
            var orderPage = new OrderPage(productID);
            await Navigation.PushModalAsync(orderPage, false);
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}