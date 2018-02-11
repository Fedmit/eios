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
        Occupation occupation;

        public CompletedOccupationPage(Occupation occupation)
		{
			InitializeComponent ();

            var viewModel = new CompletedOccupationListViewModel(occupation);
            BindingContext = viewModel;

            this.occupation = occupation;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;
            };
        }

        async void OnEditClicked(Object sender, AssemblyLoadEventArgs args)
        {
            Navigation.InsertPageBefore(new StudentsPage(this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}