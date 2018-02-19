using eios.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace eios.iOS.Services
{
    class SyncUnsentChangesTaskService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("SyncUnsentChangesTask", OnExpiration);

            try
            {
                var task = new SyncUnsentChangesTask();
                await task.RunSyncUnsentChanges(_cts.Token);
            }
            catch (OperationCanceledException)
            {
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}
