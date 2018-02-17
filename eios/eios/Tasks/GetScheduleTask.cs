using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
            MessagingCenter.Send(new StopSyncAttendanceTaskMessage(), "StopSyncAttendanceTaskMessage");
            Console.WriteLine("-----New Date-----");
            try
            {
                isSuccessful = false;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var groups = await App.Database.GetGroups();

                    await App.Database.DropTable<Occupation>();
                    await App.Database.CreateTable<Occupation>();

                    foreach (var group in groups)
                    {
                        var occupations = await WebApi.Instance.GetOccupationsAsync(group.IdGroup);
                        await App.Database.SetOccupations(occupations);
                    }
                    isSuccessful = true;
                }
            }
            catch (HttpRequestException)
            {
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
    }
}
