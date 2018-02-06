using eios.Data;
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
        bool IsBusy { get; set; } = false;

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

        async void OnLoginButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            loginButton.IsEnabled = false;
            loadingOverlay.IsVisible = true;
            activityIndicator.IsRunning = true;

            string login = loginEntry.Text;
            string password = passwordEntry.Text;

            bool isValid = await AreCredentialsCorrect(login, password);
            if (isValid)
            {
                App.Current.Properties["IsLoggedIn"] = true;
                App.Current.Properties["Login"] = login;
                App.Current.Properties["Password"] = password;
                await App.Current.SavePropertiesAsync();

                Application.Current.MainPage = new MainPage();
            }
            else
            {
                await ShowMessage("", "Пароль или логин введены неверно!", "OK");
                Console.WriteLine("Login failed");
            }

            loginButton.IsEnabled = true;
            loadingOverlay.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async Task<bool> AreCredentialsCorrect(string login, string password)
        {
            var groups = await WebApi.Instance.GetGroupsAsync(login, password);

            if (groups != null)
            {
                App.Groups = groups;
                App.Current.Properties["IdGroupCurrent"] = groups[0].IdGroup;

                return true;
            }
            else { return false; }
        }
    }
}