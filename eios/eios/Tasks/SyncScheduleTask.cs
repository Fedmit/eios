using eios.Data;
using eios.Messages;
using eios.Model;
using System;
using System.Collections.Generic;
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
            if (App.IsConnected)
            {
                try
                {
                    DateTime dateNow = await WebApi.Instance.GetDateAsync();
                    string dateNowStr = dateNow.ToString("yyyy-mm-dd HH:mm:ss");

                    string lastDate = null;
                    if (App.Current.Properties.ContainsKey("DateNow"))
                    {
                        lastDate = (string)App.Current.Properties["DateNow"];
                    }

                    if (lastDate == null || lastDate != dateNowStr)
                    {
                        var groups = await App.Database.GetGroups();

                        await App.Database.DeleteThisShits();
                        await App.Database.CreateTables();
                        foreach (var group in groups)
                        {
                            var occupations = await WebApi.Instance.GetOccupationsAsync(group.IdGroup);
                            var students = await WebApi.Instance.GetStudentsAsync(group.IdGroup);

                            await App.Database.SetOccupations(occupations);
                            await App.Database.SetStudents(students);
                        }
                    }
                    App.Current.Properties["DateNow"] = dateNowStr;
                    await App.Current.SavePropertiesAsync();

                    isSuccessful = true;
                }
                catch (HttpRequestException)
                {
                    isSuccessful = false;
                }
            }

            var message = new OnScheduleSyncronizedMessage()
            {
                IsSuccessful = isSuccessful
            };
            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send(message, "OnScheduleSyncronizedMessage");
            });

            App.IsLoading = false;
        }
    }
}
