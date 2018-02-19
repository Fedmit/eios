using eios.Data;
using eios.Messages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncScheduleStateTask
    {
        public async Task RunSyncScheduleState(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    Debug.WriteLine("TaskDebugger: SyncScheduleStateTask' iteration");

                    token.ThrowIfCancellationRequested();

                    if (CrossConnectivity.Current.IsConnected && !App.IsScheduleSync)
                    {
                        var marksResponse = await WebApi.Instance.GetMarksAsync();
                        if (marksResponse != null && marksResponse.Data != null)
                        {
                            await App.Database.SetMarks(marksResponse.Data, App.IdGroupCurrent);
                            App.IdOccupNow = marksResponse.IdOccupNow;

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                MessagingCenter.Send(new OnMarksUpdatedMessage(), "OnMarksUpdatedMessage");
                            });
                        }
                    }

                    await Task.Delay(5000);
                }
            }, token);
        }
    }
}