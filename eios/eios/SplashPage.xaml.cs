using eios.Data;
using eios.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using eios.Model;

namespace eios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashPage : ContentPage
	{
        public string Login
        {
            get
            {
                if (App.Current.Properties.ContainsKey("Login"))
                {
                    return (string)App.Current.Properties["Login"];
                }
                return "";
            }
        }

        public string Password
        {
            get
            {
                if (App.Current.Properties.ContainsKey("Password"))
                {
                    return (string)App.Current.Properties["Password"];
                }
                return "";
            }
        }

        public SplashPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            //Тестирую бд
            base.OnAppearing();

            if (App.IsConnected)
            {
                await Task.Delay(3000);

                if (App.Current.Properties.ContainsKey("IsLoggedIn") && (bool)App.Current.Properties["IsLoggedIn"])
                {
                    MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
                    MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");

                    App.Current.MainPage = new MainPage();
                }
                else
                {
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync();
                }
            }
        }
    }
}