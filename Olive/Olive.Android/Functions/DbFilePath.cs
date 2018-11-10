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

namespace Olive.Droid.Functions
{
    public class DbFilePath : IDbFilePath
    {
        public string GetLocalFilePath()
        {
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return libraryPath;
        }
    }
}