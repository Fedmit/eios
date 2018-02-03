using eios.Model;
using eios.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios
{
	public partial class App : Application
	{
        public const string DATABASE_NAME = "occupation.db";
        public static OccupationsRepository database;
        public static OccupationsRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new OccupationsRepository(DATABASE_NAME);
                }
                return database;
            }
        }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static bool IsUserLoggedIn { get; set; }

        public static DateTime Date { get; set; }

        public static List<Group> Groups { get; set; }

        public App ()
		{
			InitializeComponent();

            Properties["Login"] = "test";
            Properties["Password"] = "test1";

            MainPage = new NavigationPage(new SplashPage());
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
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
