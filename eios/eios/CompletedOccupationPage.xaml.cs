using eios.Model;
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
	public partial class CompletedOccupationPage : ContentPage
	{
        public CompletedOccupationPage(Occupation occupation)
		{
			InitializeComponent ();

            var viewModel = new CompletedOccupationListViewModel(occupation);
            BindingContext = viewModel;
        }

        async void OnBackClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}