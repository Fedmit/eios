using eios.Data;
using eios.Messages;
using eios.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncScheduleTask
    {
        public async Task RunSyncSchedule()
        {
            App.IsLoading = true;

            if (App.IsConnected)
            {
                DateTime dateNow = await WebApi.Instance.GetDateAsync();
                string dateNowStr = dateNow.ToString("yyyy:mm:dd");
                if (App.Current.Properties.ContainsKey("DateNow") && (string)App.Current.Properties["DateNow"] != dateNowStr)
                {
                        
                }
                App.Current.Properties["DateNow"] = dateNowStr;
            }

            App.IsLoading = false;

            var message = new OnScheduleSyncronizedMessage()
            {
                IsSuccessful = true
            };
            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });
        }
    }
}
