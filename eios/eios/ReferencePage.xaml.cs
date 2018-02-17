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
    public partial class ReferencePage : ContentPage
    {
        public ReferencePage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (App.IsUserLoggedIn)
            {
                App.Current.MainPage = new MainPage();
                return true;
            }
            else
            {
                base.OnBackButtonPressed();
                return false;
            }
        }
    }
}