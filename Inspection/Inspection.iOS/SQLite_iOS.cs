using System;
using System.IO;
using SQLite;
using Inspection.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace Inspection.iOS
{
    public class SQLite_iOS :ISQLite
    {


        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var fileName = "Inspection.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);

            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);

            return connection;
        }

    }
}