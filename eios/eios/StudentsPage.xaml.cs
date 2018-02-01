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
        public StudentsPage(DateTime time, string name)
        {
            InitializeComponent();

            var studentsViewModel = new StudentsListViewModel(time, name);
            BindingContext = studentsViewModel;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is Student item)
                {
                    item.IsSelected = !item.IsSelected;
                }
            };

        }
    }
}