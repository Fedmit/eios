using eios.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public bool mark = false;
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
                
                if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
                {
                    Account account = new Account
                    {
                        Username = login
                    };
                    account.Properties.Add("Password", password);
                    AccountStore.Create().Save(account, "eios");
                }

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
    }
}