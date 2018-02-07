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
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static bool IsUserLoggedIn { get; set; }
        public static bool IsLoading { get; set; } = false;
        public static bool IsConnected { get; set; }

        public static DateTime DateNow { get; set; }

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

        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new SplashPage());
		}

        protected override void OnStart()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;
            Debug.WriteLine($"Connectivity is {isConnected}");
            IsConnected = isConnected;

            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                Debug.WriteLine($"Connectivity changed to {args.IsConnected}");
                IsConnected = args.IsConnected;
            };
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
