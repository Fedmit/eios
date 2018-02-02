using eios.Data;
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
        public bool mark=false;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void OnLoginButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            string login = loginEntry.Text;
            string password = passwordEntry.Text;

            bool isValid = await AreCredentialsCorrect(login, password);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                App.Login = login;
                App.Password = password;

                Application.Current.MainPage = new MainPage();
            }
            else
            {
                Console.WriteLine("Login failed");
            }
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

        void ButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            if (mark == false)
            {
                mark = true;
                rem.Image = "ch.png";
            }
            else
            {
                mark = false;
                rem.Image = "unch.png";
            }
        }

    }
}