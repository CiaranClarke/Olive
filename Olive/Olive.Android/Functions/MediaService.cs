using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Olive.Interfaces;
using Xamarin.Forms;

namespace Olive.Droid.Functions
{
    class MediaService : IMediaService
    {
        public bool getImage()
        {
            //var context = MainActivity.Instance;
            ////((Activity)Forms.Context).StartActivityForResult(
            ////            MainActivity.selectImage1Click);
            //context.selectImage1Click();
            return true;
        }
    }
}