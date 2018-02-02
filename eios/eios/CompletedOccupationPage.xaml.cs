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
        public CompletedOccupationPage(DateTime time, string name, int id)
		{
			InitializeComponent ();

            occupationTime.Text = time.ToString("HH:mm");
            occupationName.Text = name;

		}

        async void COPageButton_isCliced(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}