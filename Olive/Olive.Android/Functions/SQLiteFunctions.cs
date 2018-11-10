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
using SQLite.Net;

namespace Olive.Droid.Functions
{
    public class SQLiteFunctions : ISQLiteFunctions
    {
        public bool TableExists(String tableName, SQLiteConnection connection)
        {
            try
            {
                SQLiteCommand cmd = connection.CreateCommand("SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name", ("@name", tableName));

                return true;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }
    }
}