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
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}

        async void OnExitButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            App.Current.Properties["IsLoggedIn"] = false;
            await App.Current.SavePropertiesAsync();

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
	}
}