using eios.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace eios.iOS.Services
{
    class SyncUnsentChangesTaskService
    {
        nint _taskId;

        public async Task Start()
        {
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("SyncUnsentChangesTask", OnExpiration);

            var task = new SyncUnsentChangesTask();
            await task.RunSyncUnsentChanges();

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        void OnExpiration()
        {
        }
    }
}
