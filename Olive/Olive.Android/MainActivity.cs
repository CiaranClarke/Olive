using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using System.IO;
using System.Collections.Generic;
using Plugin.CurrentActivity;

namespace Olive.Droid
{
    [Activity(Label = "Olive", Icon = "@mipmap/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Image image1;
        //internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Instance = this;
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //Bitmap bitmap = (Bitmap)data.Extras.Get("data");

            //byte[] bitmapData;
            //using (var stream = new MemoryStream())
            //{
            //    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            //    bitmapData = stream.ToArray();
            //}
            //Stream stream1 = new MemoryStream(bitmapData);
            //var imageSource1 = ImageSource.FromStream(() => new MemoryStream(bitmapData));
            //image1.Source = imageSource1;

            //MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "ImagesSelected", image1);
        }

        //public void selectImage1Click()
        //{
        //    Intent intent = new Intent(MediaStore.ActionImageCapture);
        //    StartActivityForResult(intent, 0);
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}