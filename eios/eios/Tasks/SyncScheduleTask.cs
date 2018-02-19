using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncScheduleTask
    {
        private bool isSuccessful;

        public async Task RunSyncSchedule()
        {
            Debug.WriteLine("TaskDebugger: SyncScheduleTask");

            try
            {
                var occupationsFromDB = await App.Database.GetOccupations(App.IdGroupCurrent);
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (occupationsFromDB == null || App.LastDate == DateTime.MinValue || App.LastDate != App.DateNow)
                    {
                        var groups = await App.Database.GetGroups();

                        await App.Database.DropTable<Student>();
                        await App.Database.DropTable<Occupation>();
                        await App.Database.CreateTable<Student>();
                        await App.Database.CreateTable<Occupation>();

                        foreach (var group in groups)
                        {
                            var occupations = await WebApi.Instance.GetOccupationsAsync(group.IdGroup);
                            var students = await WebApi.Instance.GetStudentsAsync(group.IdGroup);

                            await App.Database.SetOccupations(occupations);
                            await App.Database.SetStudents(students);
                        }
                    }
                }
                isSuccessful = true;
            }
            catch (HttpRequestException)
            {
                isSuccessful = false;
            }

            var message = new OnScheduleSyncronizedMessage()
            {
                IsSuccessful = isSuccessful,
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });

            App.IsScheduleSync = false;

            App.IsAttendanceSync = true;
            MessagingCenter.Send(new StartSyncAttendanceTaskMessage(), "StartSyncAttendanceTaskMessage");
        }
    }
}
