using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Olive.AppSpecific;
using Newtonsoft.Json;
using Firebase.Database;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.IO;

namespace Olive.ViewModels
{
    public class WishlistViewModel
    {
        private List<ItemProduct> _myWishlist;
        //private static List<tblIphoneIncidents> incidentsList;
        public static bool hasWishlist = true;
        public event PropertyChangedEventHandler PropertyChanged;
        List<string> prodNoImages = new List<string>();
        List<tblWishlist> wishlistNumbers = new List<tblWishlist>();
        List<tblProducts> productList = new List<tblProducts>();
        List<tblProductImages> imageList = new List<tblProductImages>();
        private List<FirebaseObject<tblProducts>> productsWishlist;
        private List<FirebaseObject<tblProductImages>> productImages;

        public WishlistViewModel()   
        {
            LoadData();
        }

        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCultureChanged(CultureInfo culture)
        {
        }

        public List<ItemProduct> WishlistItems
        {
            get { return _myWishlist; }
            set { SetProperty(ref _myWishlist, value); }
        }

        private void LoadData()
        {
            //var cases = CreateCases();
            //foreach(var case1 in cases)
            //{
            //    _myCases.Add(case1);
            //}

            //_myWishlist = CreateWishlist();
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

            foreach (var e in wishlist)
            {
                var json = JsonConvert.SerializeObject(e.Object);
                var wishObject = JsonConvert.DeserializeObject<tblWishlist>(json);
                wishlistNumbers.Add(wishObject);
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
                //string str_JSONincident =  AppServiceLiveToPhone.GetLiveDBIncidents(Convert.ToInt32(Settings.LiveIpCoID)).Result;
                //string str_JSONincident = AppServiceLiveToPhone.getLiveDBIncidentsSync(Convert.ToInt32(Settings.LiveIpCoID)).Result;

                //if (str_JSONincident != "0")
                //{
                //    foreach (LiveIncidents item in JsonConvert.DeserializeObject<List<LiveIncidents>>(str_JSONincident))
                //    {
                //        result.Add(
                //           new MyCases
                //           {
                //               Id = item.IpInID,
                //               Name = "Case No. " + item.IpInID.ToString(),
                //               BackgroundColor = Color.FromHex("#00162E"),
                //               BackgroundImage = "",
                //               Icon = GrialShapesFont.Event,
                //               Badge = 2,
                //               CaseInfo = item.IpInDateAdded.ToString()
                //           });
                //    }
                //hasWishlist = true;
                //}
                //else
                //{
                //    hasWishlist = false;
                //}
                await CreateWishlist(Settings.email, Settings.password);
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async static void GetIncidents()
        //{
        //    await AppServiceLiveToPhone.GetLiveDBIncidents(Convert.ToInt32(Settings.LiveIpCoID));
        //}

    }
}
