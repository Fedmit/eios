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

        void OnLoginButtonClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var isValid = true;
            if (isValid)
            {
                Application.Current.MainPage = new StudentsPage();
            }
        }
    }
}