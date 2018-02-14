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
                IsFirstTime = false
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });

            App.IsLoading = false;
        }
    }
}
