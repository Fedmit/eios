using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using eios.Messages;
using eios.Droid.Services;
using Android.Content;

namespace eios.Droid
{
    [Activity(Label = "eios", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            WireUpTask();
        }

        void WireUpTask()
        {
            MessagingCenter.Subscribe<StartSyncScheduleTaskMessage>(this, "StartSyncScheduleTaskMessage", message => {
                var intent = new Intent(this, typeof(SyncScheduleTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StartSyncScheduleStateTaskMessage>(this, "StartSyncScheduleStateTaskMessage", message => {
                var intent = new Intent(this, typeof(SyncScheduleStateTaskService));
                StartService(intent);
            });
            MessagingCenter.Subscribe<StopSyncScheduleStateTaskMessage>(this, "StopSyncScheduleStateTaskMessage", message => {
                var intent = new Intent(this, typeof(SyncScheduleStateTaskService));
                StopService(intent);
            });

            MessagingCenter.Subscribe<StartSyncUnsentChangesTask>(this, "StartSyncUnsentChangesTask", message => {
                var intent = new Intent(this, typeof(SyncUnsentChangesTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StartGetScheduleTaskMessage>(this, "StartGetScheduleTaskMessage", message => {
                var intent = new Intent(this, typeof(GetScheduleTaskService));
                StartService(intent);
            });
        }
    }
}

