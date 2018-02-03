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
        int IdOccupation { get; set; }
        DateTime date; string nameOccupation; int idOccupation;

        public InfoStudents(DateTime date, string nameOccupation, int idOccupation)
        {
            this.date = date;
            this.nameOccupation = nameOccupation;
            this.idOccupation = idOccupation;
            InitializeComponent();

            viewModel = new StudentsListViewModel(date, nameOccupation);
            BindingContext = viewModel;

            IdOccupation = idOccupation;

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