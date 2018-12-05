using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Olive.Droid.Functions;
using Olive.Interfaces;
using SQLite.Net;

[assembly: Xamarin.Forms.Dependency(typeof(ConnectionSQLite))]
namespace Olive.Droid.Functions
{
    public class ConnectionSQLite : IConnectionSQLite
    {
        public ConnectionSQLite()
        {
        }

        public SQLiteConnection CreateConnection()
        {
            //var sqliteFilename = "Olive.db3";
            //string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            //string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            //var path = Path.Combine(libraryPath, sqliteFilename);

            var dbName = "Olive.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var param = new SQLiteConnectionString(path, false);
            var connection = new SQLiteConnection(platform, path);
            return connection;
        }
    }
}