using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
                await App.Database.DropTable<StudentAbsent>();
                await App.Database.CreateTable<StudentAbsent>();

                var occupations = await App.Database.GetOccupations(App.IdGroupCurrent);
                if (occupations != null)
                {
                    foreach (var occupation in occupations)
                    {
                        await SyncAttendance(token, occupation.IdOccupation, App.IdGroupCurrent);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(new OnAttendanceSyncronizedMessage(), "OnAttendanceSyncronizedMessage");
                    });
                }
                App.IsAttendanceSync = false;
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    var unblockedOccups = await App.Database.GetUnblockedOccupations(App.IdGroupCurrent);
                    if (unblockedOccups != null)
                    {
                        foreach (var occupation in unblockedOccups)
                        {
                            await SyncAttendance(token, occupation.IdOccupation, App.IdGroupCurrent);
                        }
                    }

                    await Task.Delay(15000);
                }
            }, token);
        }

        async Task SyncAttendance(CancellationToken token, int idOccupation, int idGroup)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                if (CrossConnectivity.Current.IsConnected)
                {
                    var attendance = await WebApi.Instance.GetAttendanceAsync(idOccupation, idGroup);
                    await App.Database.SetAttendence(attendance, idOccupation, idGroup);
                    return;
                }

                await Task.Delay(5000);
            }
        }
    }
}
