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

            datePicker.MaximumDate = App.DateNow;
            datePicker.MinimumDate = App.DateNow.AddDays(-5);
            datePicker.Date = App.DateSelected;
            datePicker.DateSelected += OnDateSelected;

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
            var message = new StartSyncScheduleStateTaskMessage();
            MessagingCenter.Send(message, "StartSyncScheduleStateTaskMessage");
        }

        protected override void OnDisappearing()
        {
            var message = new StopSyncScheduleStateTaskMessage();
            MessagingCenter.Send(message, "StopSyncScheduleStateTaskMessage");
        }

        void OnDateClicked(Object sender, DateChangedEventArgs e)
        {
            if (!App.IsScheduleSync)
            {
                datePicker.Focus();
            }
        }

        void OnGroupClicked(Object sender)
        {
            if (!App.IsScheduleSync)
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
            if (!App.IsScheduleSync)
            {
                if (e.NewDate == App.DateSelected)
                {
                    return;
                }
                else if (CrossConnectivity.Current.IsConnected && e.NewDate != App.DateSelected)
                {
                    App.DateSelected = e.NewDate;
                    await App.Current.SavePropertiesAsync();

                    ViewModel.IsBusy = true;
                    ViewModel.Date = e.NewDate.ToString("dd/MM/yyyy") + "  ▼";
                    App.IsScheduleSync = true;
                    MessagingCenter.Send(new StartGetScheduleTaskMessage(), "StartGetScheduleTaskMessage");
                }
                else
                {
                    await DisplayAlert(
                                "Ошибка",
                                "Вы не подключены!",
                                "ОК");
                        datePicker.Date = App.DateSelected;
                }
            }
        }
    }
}