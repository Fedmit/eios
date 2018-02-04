using eios.Data;
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
    public partial class InfoStudents : ContentPage
    {
        StudentsListViewModel viewModel;

        Occupation occupation;

        public InfoStudents(Occupation occupation)
        {
            InitializeComponent();

            viewModel = new StudentsListViewModel(occupation);
            BindingContext = viewModel;

            this.occupation = occupation;

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
    }
}