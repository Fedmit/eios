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
            base.OnAppearing();

            if (await WebApi.CheckNetworkConnection())
            {
                await Task.Delay(5000);

                if (App.Current.Properties.ContainsKey("IsLoggedIn") && (bool)App.Current.Properties["IsLoggedIn"])
                {
                    bool isValid = await AreCredentialsCorrect(Login, Password);
                    if (isValid)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                    else
                    {
                        Console.WriteLine("Login failed");
                    }
                    App.Current.MainPage = new MainPage();
                }
                else
                {
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync();
                }
            }
        }

        private async Task<bool> AreCredentialsCorrect(string login, string password)
        {
            var groups = await WebApi.Instance.GetGroupsAsync(login, password);

            if (groups != null)
            {
                App.Groups = groups;

                return true;
            }
            else { return false; }
        }
    }
}