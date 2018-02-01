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
    
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent ();

            //if (!Application.Current.Properties.ContainsKey("Login"))
            //{
            //    Application.Current.Properties.Add("Login", "test");
            //}
            //if (!Application.Current.Properties.ContainsKey("Password"))
            //{
            //    Application.Current.Properties.Add("Password", "test1");
            //}
        }

       
    }
}