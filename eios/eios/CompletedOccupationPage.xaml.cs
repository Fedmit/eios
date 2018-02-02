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
        public CompletedOccupationPage(DateTime date, string nameOccupation, int idOccupation)
		{
			InitializeComponent ();

            var viewModel = new CompletedOccupationListViewModel(date, nameOccupation, idOccupation);
            BindingContext = viewModel;
        }

        async void OnBackClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}