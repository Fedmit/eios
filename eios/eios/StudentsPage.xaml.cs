using eios.Model;
using eios.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        StudentsListViewModel viewModel;

        public StudentsPage(DateTime time, string name)
        {
            InitializeComponent();

            viewModel = new StudentsListViewModel(time, name);
            BindingContext = viewModel;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is Student item)
                {
                    item.IsSelected = !item.IsSelected;
                    viewModel.OnSite = viewModel.StudentsList.FindAll(s => s.IsSelected.Equals(true)).Count;
                }
            };

        }

        async void OnBackClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }

        void OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}