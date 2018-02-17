using eios.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace eios.iOS.Services
{
    class GetScheduleTaskService
    {
        nint _taskId;

        public async Task Start()
        {
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("SyncScheduleTask", OnExpiration);

            var task = new GetScheduleTask();
            await task.RunGetSchedule();

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        void OnExpiration()
        {
        }
    }
}
