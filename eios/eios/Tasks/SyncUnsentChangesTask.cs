using eios.Data;
using eios.Messages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
                    if (unsentOccupations != null)
                    {
                        foreach (var occupation in unsentOccupations)
                        {
                            if (!CrossConnectivity.Current.IsConnected)
                            {
                                return;
                            }
                            var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                            if (App.Current.Properties.ContainsKey("DateNow"))
                            {
                                try
                                {
                                    await WebApi.Instance.SetAttendAsync(students, occupation);
                                    await App.Database.SetSentFlag(occupation.IdOccupation, App.IdGroupCurrent);
                                }
                                catch (HttpRequestException)
                                {
                                    await App.Database.DeleteAttendance(occupation.IdOccupation, App.IdGroupCurrent);
                                    await App.Database.SetSentFlag(occupation.IdOccupation, App.IdGroupCurrent);
                                }
                            }
                        }
                    }
                }

                await Task.Delay(10000);
            }
        }
    }
}
