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
    public class GetScheduleTaskService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => {
                var task = new GetScheduleTask();
                task.RunGetSchedule().Wait();
            });

            return StartCommandResult.Sticky;
        }
    }
}