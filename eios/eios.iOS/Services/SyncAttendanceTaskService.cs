using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eios.Tasks;
using Foundation;
using UIKit;

namespace eios.iOS.Services
{
    class SyncAttendanceTaskService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("SyncScheduleStateTask", OnExpiration);

            try
            {
                var task = new SyncAttendanceTask();
                await task.RunSyncAttendance(_cts.Token);
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