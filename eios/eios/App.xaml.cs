﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace eios
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            Properties.Add("Login", "test");
            Properties.Add("Password", "test1");

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
