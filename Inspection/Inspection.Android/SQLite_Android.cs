using System;
using System.IO;
using Xamarin.Forms;
using Inspection.Droid;

[assembly: Dependency(typeof(SQLite_Android))]
namespace Inspection.Droid
{
    public class SQLite_Android : ISQLite
    {
         public SQLite.Net.SQLiteConnection GetConnection()  
        {  
            var filename = "Inspection.db3";  
            var documentspath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);  
            var path = Path.Combine(documentspath, filename);  
   
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();  
            var connection = new SQLite.Net.SQLiteConnection(platform, path);             
            return connection;  
        }     
    }
}