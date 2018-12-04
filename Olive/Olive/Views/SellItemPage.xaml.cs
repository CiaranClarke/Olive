using Olive.Interfaces;
using Plugin.Media;
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
    public partial class SellItemPage : ContentPage
    {
        public SellItemPage()
        {
            //Navigation.RemovePage(new MainPage1());
            NavigationPage.SetHasNavigationBar(this, true);
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
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                selectImage1.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

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

                selectImage1.Source = ImageSource.FromStream(() =>
                {
                    var stream = file1.GetStream();
                    file1.Dispose();
                    return stream;
                });
            }
            
        }
    }
}