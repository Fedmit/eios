using eios.Data;
using eios.Model;
using eios.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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

        Occupation occupation;

        public StudentsPage(Occupation occupation)
        {
            InitializeComponent();

            viewModel = new StudentsListViewModel(occupation);
            BindingContext = viewModel;

            this.occupation = occupation;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is StudentSelect item)
                {
                    item.IsSelected = !item.IsSelected;
                    viewModel.OnSite = viewModel.StudentsList.FindAll(s => s.IsSelected.Equals(true)).Count;
                }
            };

        }

        async void OnUnaviableClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }

        async Task OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
            await App.Database.SetAttendence(viewModel.StudentsList, occupation.IdOccupation, idGroup);

            if (App.IsConnected)
            {
                var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                try
                {
                    await WebApi.Instance.SetAttendAsync(students, occupation);
                    await App.Database.SetSentFlag(occupation.IdOccupation, idGroup);
                }
                catch (HttpRequestException)
                {
                    await App.Database.DeleteAttendance(occupation.IdOccupation, idGroup);

                    await Navigation.PopAsync();
                    return;
                }
            }

            Navigation.InsertPageBefore(new CompletedOccupationPage(this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}