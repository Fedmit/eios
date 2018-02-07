using eios.Data;
using eios.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncUnsentChangesTask
    {

        public async Task RunSyncUnsentChanges()
        {
            if (App.IsConnected)
            {
                var unsentOccupations = await App.Database.GetUnsentOccupations();
                if(unsentOccupations != null)
                {
                    foreach(var occupation in unsentOccupations)
                    {
                        var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                        if (students != null && App.Current.Properties.ContainsKey("DateNow"))
                        {
                            if (!await WebApi.Instance.SetAttendAsync(1, students))
                            {
                                if (App.IsConnected)
                                {
                                    var attendance = await WebApi.Instance.GetAttendanceAsync(occupation.IdOccupation, occupation.IdGroup);
                                    if (attendance != null)
                                    {
                                        await App.Database.SetAttendence(attendance, occupation.IdOccupation, occupation.IdGroup);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
