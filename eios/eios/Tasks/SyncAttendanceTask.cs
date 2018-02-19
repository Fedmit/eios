using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncAttendanceTask
    {
        public async Task RunSyncAttendance(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                Debug.WriteLine("TaskDebugger: SyncAttendanceTask");

                await App.Database.DropTable<StudentAbsent>();
                await App.Database.CreateTable<StudentAbsent>();

                var occupations = await App.Database.GetOccupations(App.IdGroupCurrent);
                if (occupations != null)
                {
                    foreach (var occupation in occupations)
                    {
                        await SyncAttendance(token, occupation.IdOccupation, App.IdGroupCurrent);
                        App.IsAttendanceSync = true;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(new OnAttendanceSyncronizedMessage(), "OnAttendanceSyncronizedMessage");
                    });
                }
                App.IsAttendanceSync = false;
                while (true)
                {
                    Debug.WriteLine("TaskDebugger: SyncAttendanceTask' iteration");

                    token.ThrowIfCancellationRequested();

                    var unblockedOccups = await App.Database.GetUnblockedOccupations(App.IdGroupCurrent);

                    await Task.Delay(15000);

                    if (unblockedOccups != null)
                    {
                        foreach (var occupation in unblockedOccups)
                        {
                            await SyncAttendance(token, occupation.IdOccupation, App.IdGroupCurrent);
                        }
                    }
                }
            }, token);
        }

        async Task SyncAttendance(CancellationToken token, int idOccupation, int idGroup)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                var isSync = await App.Database.IsSync(idOccupation, idGroup);

                if (CrossConnectivity.Current.IsConnected & isSync)
                {
                    try
                    {
                        var attendance = await WebApi.Instance.GetAttendanceAsync(idOccupation, idGroup);
                        await App.Database.SetAttendence(attendance, idOccupation, idGroup);
                        return;
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(new OnAttendanceSyncronizedMessage(), "OnAttendanceSyncronizedMessage");
                });
                App.IsAttendanceSync = false;
                await Task.Delay(5000);
            }
        }
    }
}
