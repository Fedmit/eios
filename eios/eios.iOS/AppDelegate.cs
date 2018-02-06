using System;
using System.Collections.Generic;
using System.Linq;
using eios.iOS.Services;
using eios.Messages;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace eios.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            WireUpTask();

            return base.FinishedLaunching(app, options);
        }

        void WireUpTask()
        {
            SyncScheduleTaskService syncScheduleTask;
            MessagingCenter.Subscribe<StartSyncScheduleTaskMessage>(this, "StartSyncScheduleTaskMessage", async message => {
                syncScheduleTask = new SyncScheduleTaskService();
                await syncScheduleTask.Start();
            });

            SyncScheduleStateTaskService syncScheduleStateTask;
            syncScheduleStateTask = new SyncScheduleStateTaskService();
            MessagingCenter.Subscribe<StartSyncScheduleStateTaskMessage>(this, "StartSyncScheduleStateTaskMessage", async message => {
                await syncScheduleStateTask.Start();
            });
            MessagingCenter.Subscribe<StopSyncScheduleStateTaskMessage>(this, "StopSyncScheduleStateTaskMessage", message => {
                syncScheduleStateTask.Stop();
            });

            SyncUnsentChangesTaskService syncUnsentChangesTask;
            MessagingCenter.Subscribe<StartSyncUnsentChangesTask>(this, "StartSyncUnsentChangesTask", async message => {
                syncUnsentChangesTask = new SyncUnsentChangesTaskService();
                await syncUnsentChangesTask.Start();
            });
        }
    }
}
