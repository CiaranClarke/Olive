using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Olive.Droid.Functions;
using Olive.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileMgr))]
namespace Olive.Droid.Functions
{
    class FileMgr : IFileMgr
    {
        public string GetDocumentPath()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return path;
        }

        public string GetImagesPath()
        {
            string path = string.Format("{0}/Images/", GetDocumentPath());
            return path;
        }

        public string GetBase64ImageString(string filePath)
        {
            string res = null;

            try
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                res = Convert.ToBase64String(b);
            }
            catch (Exception)
            {
                //Lgc_Logger.Error(ex.Message);
            }
            return res;
        }

        public Task<string> GetBase64ImageStringAsync(string filename)
        {
            return null;
        }
    }
}