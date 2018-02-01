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
	public partial class CompletedOccupationPage : ContentPage
	{
		public CompletedOccupationPage ()
		{
			InitializeComponent ();
		}

        async void COPageButton_isCliced(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}