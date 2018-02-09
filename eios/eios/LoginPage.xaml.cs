using eios.Data;
using eios.Messages;
using eios.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async Task ShowMessage(string message, string title, string buttonText)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);
        }

        void OnLoginButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            loginButton.IsEnabled = false;
            loadingOverlay.IsVisible = true;
            activityIndicator.IsRunning = true;

            App.Login = loginEntry.Text;
            App.Password = passwordEntry.Text;

            MessagingCenter.Subscribe<OnGroupsLoadedMessage>(this, "OnGroupsLoadedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //MessagingCenter.Send(new StartSyncUnsentChangesTask(), "StartSyncUnsentChangesTask");

                    App.Current.Properties["IsLoggedIn"] = true;
                    App.Current.Properties["Login"] = App.Login;
                    App.Current.Properties["Password"] = App.Password;
                    await App.Current.SavePropertiesAsync();

                    Application.Current.MainPage = new MainPage();
                });
            });

            MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!message.IsSuccessful)
                    {
                        loginButton.IsEnabled = true;
                        loadingOverlay.IsVisible = false;
                        activityIndicator.IsRunning = false;

                        await ShowMessage("", "Пароль или логин введены неверно!", "OK");
                        Console.WriteLine("Login failed");
                    }
                });
            });

            MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
        }
    }
}