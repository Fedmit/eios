using System;
using Xamarin.Forms;
using System.IO;
using eios.iOS;
using eios.Data;

[assembly: Dependency(typeof(SQLite_IOS))]
namespace eios.iOS
{
    class SQLite_IOS : ISQlite
    {
        public SQLite_IOS() { }

        public string GetDatabasePath(string filename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // папка библиотеки
            var path = Path.Combine(libraryPath, filename);

            return path;
        }
    }
}
