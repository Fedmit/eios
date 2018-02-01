using System;
using System.IO;
using eios.Data;
using eios.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_IOS))]
namespace eios.iOS
{
    class SQLite_IOS : ISQLite
    {
        public SQLite_IOS() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            // определяем путь к бд
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // папка библиотеки
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}
