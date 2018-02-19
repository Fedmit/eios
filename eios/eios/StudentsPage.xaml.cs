using eios.Data;
using eios.Messages;
using eios.Model;
using eios.ViewModel;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        StudentsListViewModel ViewModel { get; set; }
        OccupationsListViewModel OccupViewModel { get; set; }

        Occupation occupation;

        public StudentsPage(OccupationsListViewModel occupViewModel, Occupation occupation)
        {
            InitializeComponent();

            OccupViewModel = occupViewModel;
            ViewModel = new StudentsListViewModel(occupation);
            BindingContext = ViewModel;

            this.occupation = occupation;
            unavaibleButton.IsEnabled = occupation.IdLesson != 0;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is StudentSelect item)
                {
                    item.IsSelected = !item.IsSelected;
                    ViewModel.OnSite = ViewModel.StudentsList.FindAll(s => s.IsSelected.Equals(false)).Count;
                }
            };

        }

        async void OnUnaviableClicked(Object sender, AssemblyLoadEventArgs args)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    await WebApi.Instance.SetNullAttendAsync(occupation);
                }
                catch (HttpRequestException)
                {
                    return;
                }
            }
            await Navigation.PopAsync();
        }

        async Task OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            markButton.IsEnabled = false;
            await App.Database.SetAttendence(ViewModel.StudentsList, occupation.IdOccupation, App.IdGroupCurrent);

            MessagingCenter.Send(new OnMarksUpdatedMessage(), "OnMarksUpdatedMessage");

            Navigation.InsertPageBefore(new CompletedOccupationPage(OccupViewModel, this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}