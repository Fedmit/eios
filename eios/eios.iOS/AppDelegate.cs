using System;
using System.Collections.Generic;
using System.Linq;
using eios.iOS.Services;
using eios.Messages;
using Foundation;
using HockeyApp.iOS;
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
        SyncScheduleStateTaskService syncScheduleStateTask = new SyncScheduleStateTaskService();
        SyncAttendanceTaskService syncAttendanceTaskService = new SyncAttendanceTaskService();
        SyncUnsentChangesTaskService syncUnsentChangesTask = new SyncUnsentChangesTaskService();

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("db48f2e1dad14d83811e8834bb8940b3");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            WireUpTask();

            return base.FinishedLaunching(app, options);
        }

        void WireUpTask()
        {
            MessagingCenter.Subscribe<StartSyncScheduleStateTaskMessage>(this, "StartSyncScheduleStateTaskMessage", async message =>
            {
                await syncScheduleStateTask.Start();
            });
            MessagingCenter.Subscribe<StopSyncScheduleStateTaskMessage>(this, "StopSyncScheduleStateTaskMessage", message =>
            {
                syncScheduleStateTask.Stop();
            });

            MessagingCenter.Subscribe<StartSyncScheduleStateTaskMessage>(this, "StartSyncScheduleStateTaskMessage", async message =>
            {
                await syncAttendanceTaskService.Start();
            });
            MessagingCenter.Subscribe<StopSyncScheduleStateTaskMessage>(this, "StopSyncScheduleStateTaskMessage", message =>
            {
                syncAttendanceTaskService.Stop();
            });

            MessagingCenter.Subscribe<StartSyncScheduleTaskMessage>(this, "StartSyncScheduleTaskMessage", async message =>
            {
                var syncScheduleTask = new SyncScheduleTaskService();
                await syncScheduleTask.Start();
            });

            MessagingCenter.Subscribe<StartSyncUnsentChangesTask>(this, "StartSyncUnsentChangesTask", async message =>
            {
                await syncUnsentChangesTask.Start();
            });
            MessagingCenter.Subscribe<StopSyncUnsentChangesTask>(this, "StopSyncUnsentChangesTask", message =>
            {
                syncUnsentChangesTask.Stop();
            });

            MessagingCenter.Subscribe<StartGetScheduleTaskMessage>(this, "StartGetScheduleTaskMessage", async message =>
            {
                var getScheduleTask = new GetScheduleTaskService();
                await getScheduleTask.Start();
            });
        }
    }
}
