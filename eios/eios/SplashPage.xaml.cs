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
using System.Diagnostics;
using System.Net;

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
                if (App.IsUserLoggedIn && App.Login != null && App.Password != null)
                {
                    GroupResponse groupResponse = null;
                    DateTime dateNow = DateTime.MinValue;
                    try
                    {
                        groupResponse = await WebApi.Instance.GetGroupsAsync();

                        dateNow = await WebApi.Instance.GetDateAsync();
                    }
                    catch (WebException ex)
                    {
                        if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        {
                            Navigation.InsertPageBefore(new LoginPage(), this);
                            await Navigation.PopAsync();

                            return;
                        }
                    }
                    var groups = await App.Database.GetGroups();
                    if (groups != null)
                    {
                        App.Groups = groups;
                    }
                    else if (groupResponse != null && groupResponse.Data != null)
                    {
                        Debug.WriteLine("Группы не было в БД");

                        App.Groups = groupResponse.Data;
                    }

                    App.DateNow = dateNow;
                    App.LastDate = App.DateSelected;
                    App.DateSelected = dateNow;
                    await App.Current.SavePropertiesAsync();

                    App.IsScheduleSync = true;
                    MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");

                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    await Task.Delay(1000);

                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await Task.Delay(1000);

                if (App.IsUserLoggedIn)
                {
                    var groups = await App.Database.GetGroups();
                    var occupations = await App.Database.GetOccupations(App.IdGroupCurrent);

                    if (groups != null && occupations != null)
                    {
                        App.Groups = groups;
                        App.IsScheduleUpToDate = true;
                        MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");
                        MessagingCenter.Send(new StartSyncAttendanceTaskMessage(), "StartSyncAttendanceTaskMessage");
                        Application.Current.MainPage = new MainPage();
                    }
                    else
                    {
                        Debug.WriteLine("Группы не было в БД");

                        Navigation.InsertPageBefore(new LoginPage(), this);
                        await Navigation.PopAsync();
                    }
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