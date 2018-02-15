using eios.Data;
using eios.Messages;
using eios.Model;
using eios.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            TapGestureRecognizer tapEvent = new TapGestureRecognizer();
            tapEvent.Tapped += OnInfoButtonClicked;
            buttonInfo.GestureRecognizers.Add(tapEvent);
        }

        public async Task ShowMessage(string title, string message, string buttonText)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);
        }

        async void OnForgotPasswordButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await ShowMessage(
                "",
                "В случае, если был утерян доступ к учебным данным к системе ЭИОС - обратитесь в библиотеку в 3 корпусе",
                "OK"
            );
        }

        async void OnLoginButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowMessage("Ошибка", "Вы не подключены!", "OK");
                return;
            }

            loginButton.IsEnabled = false;
            activityIndicator.IsRunning = true;

            App.Login = loginEntry.Text;
            App.Password = passwordEntry.Text;

            GroupResponse response;
            try
            {
                response = await WebApi.Instance.GetGroupsAsync();
            }
            catch (HttpRequestException ex)
            {
                loginButton.IsEnabled = true;
                activityIndicator.IsRunning = false;

                await ShowMessage("Ошибка", "Пароль или логин введены неверно!", "OK");
                Console.WriteLine(ex.Message);

                return;
            }

            await App.Database.DropTable<Group>();
            await App.Database.CreateTable<Group>();
            await App.Database.SetGroup(response.Data);

            App.Groups = response.Data;

            App.IdGroupCurrent = response.Data[0].IdGroup;
            App.IsUserLoggedIn = true;

            App.Current.Properties["Login"] = App.Login;
            App.Current.Properties["Password"] = App.Password;

            await App.Current.SavePropertiesAsync();

            App.IsScheduleSync = true;
            MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");

            Application.Current.MainPage = new MainPage();
        }

        private void OnInfoButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReferencePage());
        }
    }
}