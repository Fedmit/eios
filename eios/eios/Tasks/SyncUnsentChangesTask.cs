using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncUnsentChangesTask
    {
        public async Task RunSyncUnsentChanges(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    Debug.WriteLine("TaskDebugger: SyncUnsentChangesTask' iteration");

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        var unsyncOccupations = await App.Database.GetUnsyncOccupations();

                        if (unsyncOccupations != null)
                        {
                            foreach (var occupation in unsyncOccupations)
                            {
                                await SyncAttendance(token, occupation);
                            }
                        }

                        if (!App.IsScheduleUpToDate)
                        {
                            App.IsScheduleUpToDate = true;
                            MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
                        }
                    }

                    await Task.Delay(3000);
                }
            }, token);
        }

        async Task SyncAttendance(CancellationToken token, Occupation occupation)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                if (CrossConnectivity.Current.IsConnected)
                {
                    var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                    await WebApi.Instance.SetAttendAsync(students, occupation);
                    await App.Database.SetSyncFlag(occupation.IdOccupation, occupation.IdGroup);
                    return;
                }

                await Task.Delay(5000);
            }
        }
    }
}
