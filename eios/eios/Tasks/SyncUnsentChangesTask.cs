﻿using eios.Data;
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
        int idGroup = (int)App.Current.Properties["IdGroupCurrent"];

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
                                    await App.Database.SetSentFlag(occupation.IdOccupation, idGroup);
                                }
                                catch (HttpRequestException)
                                {
                                    await App.Database.DeleteAttendance(occupation.IdOccupation, idGroup);
                                    await App.Database.SetSentFlag(occupation.IdOccupation, idGroup);
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
