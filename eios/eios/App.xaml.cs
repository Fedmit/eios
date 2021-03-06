﻿using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using eios.Data;
using Xamarin.Auth;

namespace eios
{
    public partial class App : Application
    {
        private static string _login { get; set; }
        public static string Login
        {
            get
            {
                if (_login == null)
                {
                    var account = AccountStore.Create().FindAccountsForService("eios").FirstOrDefault();
                    _login = account?.Username;
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
                    var account = AccountStore.Create().FindAccountsForService("eios").FirstOrDefault();
                    _password = account?.Properties["Password"];
                }
                return _password;
            }
            set { _password = value; }
        }

        public static bool IsUserLoggedIn
        {
            get
            {
                if (Current.Properties.ContainsKey("IsUserLoggedIn"))
                {
                    return (bool) Current.Properties["IsUserLoggedIn"];
                }
                return false;
            }
            set
            {
                Current.Properties["IsUserLoggedIn"] = value;
            }
        }
        public static bool IsScheduleSync { get; set; } = false;
        public static bool IsScheduleUpToDate { get; set; } = false;
        public static bool IsAttendanceSync { get; set; } = false;
        public static bool IsUnsyncAny { get; set; } = false;
        public static int IdOccupNow { get; set; } = 8;
        public static bool IsTimeTravelMode { get; set; }

        public static DateTime DateNow
        {
            get
            {
                if (Current.Properties.ContainsKey("DateNow"))
                {
                    var dateNowStr = (string) Current.Properties["DateNow"];
                    return DateTime.Parse(dateNowStr);
                }
                return new DateTime(2020, 1, 1);
            }
            set
            {
                Current.Properties["DateNow"] = value.ToString("yyyy-MM-dd");
            }
        }

        public static DateTime DateSelected
        {
            get
            {
                if (Current.Properties.ContainsKey("DateSelected"))
                {
                    var dateNowStr = (string) Current.Properties["DateSelected"];
                    return DateTime.Parse(dateNowStr);
                }
                return DateTime.MinValue;
            }
            set
            {
                Current.Properties["DateSelected"] = value.ToString("yyyy-MM-dd");
            }
        }

        public static DateTime LastDate { get; set; }

        public static int IdGroupCurrent
        {
            get
            {
                if (Current.Properties.ContainsKey("IdGroupCurrent"))
                {
                    return (int) Current.Properties["IdGroupCurrent"];
                }
                return 0;
            }
            set
            {
                Current.Properties["IdGroupCurrent"] = value;
            }
        }

        public static List<Group> _groups;
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

            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs excArgs) =>
            {
                Console.WriteLine("Exception.Message: {0}\n", excArgs.Exception.Message);
                Console.WriteLine("Exception.InnerException.Message: {0}\n", excArgs.Exception.InnerException.Message);
                excArgs.SetObserved();
            };
        }

        protected override void OnSleep()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
