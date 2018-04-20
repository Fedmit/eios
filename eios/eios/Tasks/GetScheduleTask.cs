using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class GetScheduleTask
    {
        private bool isSuccessful;

        public async Task RunGetSchedule()
        {
            Debug.WriteLine("TaskDebugger: GetScheduleTask. New Date selected");

            MessagingCenter.Send(new StopSyncAttendanceTaskMessage(), "StopSyncAttendanceTaskMessage");
            try
            {
                isSuccessful = false;
                if (CrossConnectivity.Current.IsConnected)
                {
                    await App.Database.DropTable<Occupation>();
                    await App.Database.CreateTable<Occupation>();

                    foreach (var group in App.Groups)
                    {
                        await SyncOccupations(group.IdGroup);
                    }
                    isSuccessful = true;

                    await App.Current.SavePropertiesAsync();
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("GetScheduleTask: " + ex.Message);
            }

            var message = new OnScheduleSyncronizedMessage()
            {
                IsSuccessful = isSuccessful,
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });

            App.IsScheduleSync = false;

            App.IsAttendanceSync = true;
            MessagingCenter.Send(new StartSyncAttendanceTaskMessage(), "StartSyncAttendanceTaskMessage");
        }

        async Task SyncOccupations(int idGroup)
        {
            while (true)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        var occupations = await WebApi.Instance.GetOccupationsAsync(idGroup);
                        await App.Database.SetOccupations(occupations);
                        return;
                    }
                    catch (WebException)
                    {
                    }
                }

                await Task.Delay(5000);
            }
        }
    }
}
