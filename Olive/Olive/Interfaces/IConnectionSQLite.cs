using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.Interfaces
{
    public interface IConnectionSQLite
    {
        SQLiteConnection CreateConnection();
    }
}
