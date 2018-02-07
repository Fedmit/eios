using eios.Data;
using eios.Messages;
using eios.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Tasks
{
    class SyncScheduleTask
    {
        private Task webapi;

        public async Task RunSyncSchedule()
        {
            App.IsLoading = true;

            if (App.IsConnected)
            {
                DateTime dateNow = await WebApi.Instance.GetDateAsync();
                string dateNowStr = dateNow.ToString("yyyy:mm:dd");

                string lastDate = null;
                if (App.Current.Properties.ContainsKey("DateNow"))
                {
                    lastDate = (string)App.Current.Properties["DateNow"];
                }

                if (lastDate == null || lastDate != dateNowStr)
                {
                    var groups = await WebApi.Instance.GetGroupsAsync();
                    await App.Database.SetGroup(groups);

                    foreach(var group in groups)
                    {
                        var occupations = await WebApi.Instance.GetOccupationsAsync(group.IdGroup);
                        var students = await WebApi.Instance.GetStudentsAsync(group.IdGroup);

                        await App.Database.SetOccupations(occupations);
                        await App.Database.SetStudents(students);
                    }
                }
                App.Current.Properties["DateNow"] = dateNowStr;
            }

            App.IsLoading = false;

            var message = new OnScheduleSyncronizedMessage()
            {
                IsSuccessful = true
            };
            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });
        }
    }
}
