using System;
using eios.Droid;
using System.IO;
using Xamarin.Forms;
using eios.Data;
[assembly: Dependency(typeof(SQLite_Android))]
namespace eios.Droid
{
    class SQLite_Android : ISQlite
    {
        public SQLite_Android() { }

        public string GetDatabasePath(string filename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);
            return path;
        }
    }
}