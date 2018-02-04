using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static bool IsConnected { get; set; }

        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new SplashPage());
		}

		protected override void OnStart ()
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
