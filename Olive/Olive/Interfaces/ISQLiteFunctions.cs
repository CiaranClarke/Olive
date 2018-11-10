using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.Interfaces
{
    public interface ISQLiteFunctions
    {
        bool TableExists(string tableName, SQLiteConnection connection);
    }
}
