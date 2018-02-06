using eios.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace eios.iOS.Services
{
    class SyncScheduleTaskService
    {
        nint _taskId;

        public async Task Start()
        {
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("SyncScheduleTask", OnExpiration);

            var task = new SyncScheduleTask();
            await task.RunSyncSchedule();

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        void OnExpiration()
        {
        }
    }
}
