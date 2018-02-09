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
        public SplashPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.IsConnected)
            {
                await Task.Delay(3000);

                if (App.Current.Properties.ContainsKey("IsLoggedIn") && (bool)App.Current.Properties["IsLoggedIn"])
                {
                    MessagingCenter.Subscribe<OnGroupsLoadedMessage>(this, "OnGroupsLoadedMessage", message =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");

                            Application.Current.MainPage = new MainPage();
                        });
                    });

                    MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!message.IsSuccessful)
                            {

                                Navigation.InsertPageBefore(new LoginPage(), this);
                                await Navigation.PopAsync();
                            }
                        });
                    });

                    MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
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