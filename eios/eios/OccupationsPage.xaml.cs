using eios.Messages;
using eios.Model;
using eios.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OccupationsPage : ContentPage
    {

        OccupationsListViewModel ViewModel { get; set; }

        public OccupationsPage()
        {
            ViewModel = new OccupationsListViewModel(this);
            BindingContext = ViewModel;

            InitializeComponent();

            listView.ItemTapped += async (sender, e) =>
            {
                listView.SelectedItem = null;
                if (e.Item is Occupation item)
                {
                    if (item.TargetType == null)
                    {
                        return;
                    }
                    await Navigation.PushAsync((Page) Activator.CreateInstance(item.TargetType, ViewModel, item));
                }
            };
        }

        protected override void OnAppearing()
        {
            //var message = new StartSyncScheduleStateTaskMessage();
            //MessagingCenter.Send(message, "StartSyncScheduleStateTaskMessage");
        }

        protected override void OnDisappearing()
        {
            //var message = new StopSyncScheduleStateTaskMessage();
            //MessagingCenter.Send(message, "StopSyncScheduleStateTaskMessage");
        }

        void OnDateClicked(Object sender, DateChangedEventArgs e)
        {
            if (!App.IsLoading)
            {
                datePicker.Focus();
            }
        }

        void OnGroupClicked(Object sender)
        {
            if (!App.IsLoading)
            {
                groupPicker.Focus();
            }
        }
        async void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker) sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                ViewModel.Group = App.Groups[selectedIndex].Name;

                App.IdGroupCurrent = App.Groups[selectedIndex].IdGroup;
                await App.Current.SavePropertiesAsync();

                await ViewModel.UpdateOccupationsList();
            }
        }

        async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected && e.NewDate != App.DateNow)
            {
                await DisplayAlert(
                            "Ошибка",
                            "Вы не подключены!",
                            "ОК");
                ViewModel.Date = App.DateSelected;
            }
        }
    }
}