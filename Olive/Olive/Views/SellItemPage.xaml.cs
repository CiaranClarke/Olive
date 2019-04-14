﻿using Olive.AppSpecific;
using Olive.Interfaces;
using Plugin.Media;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellItemPage : ContentPage
    {
        List<string> _images = new List<string>();
        public static int pNo;
        FirebaseClient firebase = new FirebaseClient("https://olive-4a870.firebaseio.com/"); //Firebase Database URL  
        string productKey;
        public SellItemPage()
        {
            //Navigation.RemovePage(new MainPage1());
            InitializeComponent();
            
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public async void selectImage(object sender, EventArgs e)
        {
            //DependencyService.Get<IMediaService>().getImage();
            //MessagingCenter.Subscribe<App, Image>((App)Xamarin.Forms.Application.Current, "ImagesSelected", (s, images) =>
            //{
            //    selectImage1 = images;
            //});
            try
            {
                int imageNumber = Convert.ToInt32(((Image)sender).ClassId);
            
                var selectImage = selectImage1;
                switch (imageNumber)
                {
                    case 1:
                        selectImage = selectImage1;
                        break;
                    case 2:
                        selectImage = selectImage2;
                        break;
                    case 3:
                        selectImage = selectImage3;
                        break;
                    case 4:
                        selectImage = selectImage4;
                        break;
                    case 5:
                        selectImage = selectImage5;
                        break;
                    case 6:
                        selectImage = selectImage6;
                        break;
                    default:
                        selectImage = null;
                        break;
                }

                var action = await DisplayActionSheet("Image", "Cancel", null, "Take a New Photo", "Select Photo from File");
          
                if(action == "Take a New Photo")
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Olive",
                        Name = "image.jpg"
                    });

                    if (file == null)
                        return;

                    selectImage.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                    _images.Add(file.Path);
                }
                else if(action == "Select Photo from File")
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                        return;
                    }
                    var file1 = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                    });
                    

                    if (file1 == null)
                        return;

                    selectImage.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file1.GetStream();
                        return stream;
                    });
                    _images.Add(file1.Path);
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task CreateProductImages(string productNo, string prodImageString)
        {
            var prodImage = await firebase
              .Child("ProductImages")
              .PostAsync(new tblProductImages()
              {
                  productNo = productNo,
                  prodImageString = prodImageString
              });
            var imageKey = prodImage.Key;

            //Settings.UserKey = token;
        }

        public async Task CreateProduct(string prodCategory, string prodSubCategory, decimal prodPrice, string prodDescription, string prodSize,
            string prodColour, string prodBrand, string prodLocation, bool prodSold, string prodSellerNo)
        {
            var product = await firebase
              .Child("Products")
              .PostAsync(new tblProducts()
              {
                  prodCategory = prodCategory,
                  prodSubCategory = prodSubCategory,
                  prodPrice = prodPrice,
                  prodDescription = prodDescription,
                  prodSize = prodSize,
                  prodColour = prodColour,
                  prodBrand = prodBrand,
                  prodLocation = prodLocation,
                  prodSold = prodSold,
                  prodSellerNo = prodSellerNo
              });
            productKey = product.Key;

            //Settings.UserKey = token;
        }

        public async void nextBtnClicked(object sender, EventArgs e)
        {
            try
            {
                await CreateProduct(txt_Category.SelectedItem.ToString(), txt_SubCategory.SelectedItem.ToString(), Convert.ToDecimal(txt_Price.Text), txt_Description.Text,
                        txt_Size.Text, txt_Colour.Text, txt_Brand.Text, Settings.UserLocation,
                        false, Settings.UserKey);

                foreach (string image in _images)
                {
                    //DisplayAlert("Image URL", image.ToString(), "Ok");

                    string imageBase64 = DependencyService.Get<IFileMgr>().GetBase64ImageString(image);

                    //var sqliteFilename = "Olive.db3";
                    //string dbPath = DependencyService.Get<IDbFilePath>().GetLocalFilePath();
                    //dbPath = System.IO.Path.Combine(dbPath, sqliteFilename);
                    //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"Olive.db3");

                    //using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
                    //{
                    //    // Create product table 
                    //    db.CreateTable<tblProducts>();

                    //    // Create your new product instance
                    //    var product = new tblProducts
                    //    {
                    //        prodCategory = "",
                    //        prodSubCategory = "",
                    //        prodPrice = 11.64m,
                    //        prodDescription = txt_Description.ToString(),
                    //        prodSize = txt_Size.ToString(),
                    //        prodColour = txt_Colour.ToString(),
                    //        prodImageString = imageBase64,
                    //        prodBrand = txt_Brand.ToString(),
                    //        prodLocation = "Newry",
                    //        prodSold = false,
                    //        prodSellerNo = Settings.UserKey;
                    //    };

                    //    // Insert new product document (Id will be auto-incremented)
                    //    db.Insert(product);

                    //    pNo = product.productNo;
                    //}

                    await CreateProductImages(productKey, imageBase64);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                await DisplayAlert("Selling", "Your item is up for sale!", "OK");
                await Navigation.PopModalAsync();
            }
        }
    }
}
