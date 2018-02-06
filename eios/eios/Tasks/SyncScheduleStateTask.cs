using eios.Data;
using eios.Messages;
using System;
using System.Collections.Generic;
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
            await Task.Run(async () => {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    if (App.IsConnected)
                    {
                        var marks = await WebApi.Instance.GetMarksAsync();
                        //await App.Database.SetMarks();
                        var message = new OnMarksUpdatedMessage()
                        {
                            IsSuccessful = true
                        };
                        Device.BeginInvokeOnMainThread(() => {
                            MessagingCenter.Send(message, "OnMarksUpdatedMessage");
                        });
                    }

                    await Task.Delay(5000);
                }
            }, token);
        }
    }
}
