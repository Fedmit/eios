using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using eios.Data;
using eios.Tasks;

namespace eios.Droid.Services
{
    [Service]
    public class SyncronizeScheduleTaskService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    var task = new SyncronizeScheduleTask();
                    task.RunSyncronizeSchedule(_cts.Token).Wait();
                }
                catch (Android.OS.OperationCanceledException)
                {
                }
                finally
                {
                    //if (_cts.IsCancellationRequested)
                    //{
                    //    var message = new CancelledMessage();
                    //    Device.BeginInvokeOnMainThread(
                    //        () => MessagingCenter.Send(message, "CancelledMessage")
                    //    );
                    //}
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}