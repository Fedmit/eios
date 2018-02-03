using eios.Model;
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
        public static DateTime Date { get; set; }

        public static List<Group> Groups { get; set; }

        public App ()
		{
			InitializeComponent();

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
