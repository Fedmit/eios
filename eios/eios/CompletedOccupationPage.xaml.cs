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
        OccupationsListViewModel OccupViewModel { get; set; }

        public CompletedOccupationPage(OccupationsListViewModel occupViewModel, Occupation occupation)
        {
            OccupViewModel = occupViewModel;
            var viewModel = new CompletedOccupationListViewModel(occupation);
            BindingContext = viewModel;

            InitializeComponent();

            this.occupation = occupation;

            editButton.IsEnabled = occupation.IsBlocked != false;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;
            };
        }

        async void OnEditClicked(Object sender, AssemblyLoadEventArgs args)
        {
            Navigation.InsertPageBefore(new StudentsPage(OccupViewModel, this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}