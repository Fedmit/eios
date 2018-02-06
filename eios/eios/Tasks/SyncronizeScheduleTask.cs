using eios.Data;
using eios.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncronizeScheduleTask
    {
        public async Task RunSyncronizeSchedule(CancellationToken token)
        {
            await Task.Run(async () => {
                App.IsLoading = true;

                if (App.IsConnected)
                {
                    DateTime dateNow = await WebApi.Instance.GetDateAsync();
                    string dateNowStr = dateNow.ToString("yyyy:mm:dd");
                    if (App.Current.Properties.ContainsKey("DateNow") && (string)App.Current.Properties["DateNow"] != dateNowStr)
                    {
                        
                    }
                }

                App.IsLoading = false;
            }, token);
        }
    }
}
