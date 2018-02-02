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
    public partial class StudentsPage : ContentPage
    {
        StudentsListViewModel viewModel;
        int IdOccupation { get; set; }

        public StudentsPage(DateTime time, string name, int idOccupation)
        {
            InitializeComponent();

            viewModel = new StudentsListViewModel(time, name);
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

        async void OnBackClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }

        async Task OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var selectedList = viewModel.StudentsList.FindAll(s => s.IsSelected.Equals(true));
            var resultList = new List<SelectedStudent>();
            foreach (Student st in selectedList)
            {
                var cache = new SelectedStudent();
                cache.Id = st.Id;
                resultList.Add(cache);
            }
            await WebApi.Instance.SetAttendAsync(1, IdOccupation, resultList);
            Application.Current.MainPage = new MainPage();
        }
    }
}