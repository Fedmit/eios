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
            if (CrossConnectivity.Current.IsConnected)
            {
                List<List<Occupation>> data = new List<List<Occupation>>();
                try
                {
                    var groups = await App.Database.GetGroups();

                    foreach (var group in groups)
                    {
                        data.Add(await WebApi.Instance.GetOccupationsAsync(group.IdGroup));
                    }

                    isSuccessful = true;
                }
                catch (HttpRequestException)
                {
                    isSuccessful = false;
                }

                var message = new OccupationsMessage()
                {
                    IsSuccessful = isSuccessful,
                    Data = data
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(message, "OccupationsMessage");
                });

                App.IsLoading = false;
            }
        }
    }
}
