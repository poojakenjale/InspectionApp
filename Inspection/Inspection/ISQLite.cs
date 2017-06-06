using System;
using SQLite.Net;

namespace Inspection
{
    public interface ISQLite
     {
        SQLiteConnection GetConnection();
      
    }
}
