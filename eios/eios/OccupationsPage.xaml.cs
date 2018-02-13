﻿using eios.Messages;
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
                    await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType, item));
                }
            };
        }

        protected override void OnAppearing()
        {
            if (!App.IsLoading)
            {
                var message = new StartSyncScheduleStateTaskMessage();
                MessagingCenter.Send(message, "StartSyncScheduleStateTaskMessage");
            }
        }

        protected override void OnDisappearing()
        {
            var message = new StopSyncScheduleStateTaskMessage();
            MessagingCenter.Send(message, "StopSyncScheduleStateTaskMessage");
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