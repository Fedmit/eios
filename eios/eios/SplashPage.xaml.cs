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
using System.Net.Http;
using Plugin.Connectivity;

namespace eios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashPage : ContentPage
	{
        public SplashPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (CrossConnectivity.Current.IsConnected)
            {
                if (App.Current.Properties.ContainsKey("IsLoggedIn") && (bool)App.Current.Properties["IsLoggedIn"])
                {
                    try
                    {
                        await WebApi.Instance.GetGroupsAsync();
                    }
                    catch (HttpRequestException)
                    {
                        Navigation.InsertPageBefore(new LoginPage(), this);
                        await Navigation.PopAsync();

                        return;
                    }

                    App.Groups = await App.Database.GetGroups();

                    App.IsLoading = true;

                    MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
                    MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");

                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                if (App.Current.Properties.ContainsKey("IsLoggedIn") && (bool)App.Current.Properties["IsLoggedIn"])
                {
                    App.Groups = await App.Database.GetGroups();
                    Application.Current.MainPage = new MainPage();
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