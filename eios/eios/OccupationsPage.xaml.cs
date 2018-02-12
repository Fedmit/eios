using eios.Messages;
using eios.Model;
using eios.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            InitializeComponent();

            ViewModel = new OccupationsListViewModel(this);
            BindingContext = ViewModel;

            listView.ItemTapped += async (sender, e) =>
            {
                listView.SelectedItem = null;
                if (e.Item is Occupation item)
                {
                    if (item.TargetType == null)
                    {
                        return;
                    }
                    await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType, item));
                }
            };
        }

        protected async override void OnAppearing()
        {
            var message = new StartSyncScheduleStateTaskMessage();
            MessagingCenter.Send(message, "StartSyncScheduleStateTaskMessage");
        }

        protected override void OnDisappearing()
        {
            var message = new StopSyncScheduleStateTaskMessage();
            MessagingCenter.Send(message, "StopSyncScheduleStateTaskMessage");
        }

        void onClicked(Object sender, DateChangedEventArgs e)
        {
            datePicker.Focus();
        }

        void datePicker_DateSelected(Object sender, DateChangedEventArgs e)
        {
            ViewModel.Date = e.NewDate.ToString("dd/MM/yyyy");
        }

        void onClickedGroup(Object sender)
        {
            pickerGroup.Focus();
        }
        async void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                App.Current.Properties["IdGroupCurrent"] = App.Groups[selectedIndex].IdGroup;
                ViewModel.Group = App.Groups[selectedIndex].Name;

                await ViewModel.UpdateOccupationsList();
            }
        }
    }
}