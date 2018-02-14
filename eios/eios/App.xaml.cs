using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using eios.Data;

namespace eios
{
    public partial class App : Application
    {
        public static int Counter { get; set; }

        private static string _login { get; set; }
        public static string Login
        {
            get
            {
                if (_login == null)
                {
                    if (App.Current.Properties.ContainsKey("Login"))
                    {
                        return (string) App.Current.Properties["Login"];
                    }
                    return "";
                }

                return _login;
            }
            set { _login = value; }
        }

        private static string _password { get; set; }
        public static string Password
        {
            get
            {
                if (_password == null)
                {
                    if (App.Current.Properties.ContainsKey("Password"))
                    {
                        return (string) App.Current.Properties["Password"];
                    }
                    return "";
                }

                return _password;
            }
            set { _password = value; }
        }

        public static bool IsUserLoggedIn { get; set; }
        public static bool IsLoading { get; set; } = false;
        public static int IdOccupNow { get; set; } = 8;
        public static bool IsTimeTravelMode { get; set; } = false;
        
        public static DateTime DateNow
        {
            get
            {
                if (Current.Properties.ContainsKey("DateNow") && (string) Current.Properties["DateNow"] != null)
                {
                    var dateNowStr = (string) Current.Properties["DateNow"];
                    return DateTime.Parse(dateNowStr);
                }
                return DateTime.MinValue;
            }
        }

        public static DateTime DateSelected { get; set; }

        public static List<Group> Groups { get; set; }

        private const string DATABASE_NAME = "EIOS_DB.db";
        private static DataBaseRepository dataBase;
        public static DataBaseRepository Database
        {
            get
            {
                if (dataBase == null)
                {
                    dataBase = new DataBaseRepository(DATABASE_NAME);
                }
                return dataBase;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SplashPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
