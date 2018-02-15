﻿using eios.Data;
using eios.Messages;
using Plugin.Connectivity;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncUnsentChangesTask
    {
        public async Task RunSyncUnsentChanges()
        {
            while (true)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var unsentOccupations = await App.Database.GetUnsentOccupations();

                    if (unsentOccupations == null)
                    {
                        MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
                        return;
                    }
                    else
                    {
                        foreach (var occupation in unsentOccupations)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                                await WebApi.Instance.SetAttendAsync(students, occupation);
                                await App.Database.SetSyncFlag(occupation.IdOccupation, App.IdGroupCurrent);
                            }
                        }
                    }
                }
                else
                {
                    await Task.Delay(5000);
                }
            }
        }
    }
}
