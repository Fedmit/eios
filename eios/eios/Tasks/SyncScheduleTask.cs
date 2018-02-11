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
            try
            {
                if (App.IsConnected)
                {
                    DateTime dateNow = await WebApi.Instance.GetDateAsync();

                    DateTime lastDate = new DateTime();
                    if (App.Current.Properties.ContainsKey("DateNow"))
                    {
                        var str = (string)App.Current.Properties["DateNow"];
                        lastDate = DateTime.Parse(str);
                    }

                    if (lastDate == DateTime.MinValue || lastDate.Date != dateNow.Date)
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
                    App.Current.Properties["DateNow"] = dateNow.ToString("yyyy-mm-dd HH:mm:ss");
                    await App.Current.SavePropertiesAsync();
                }
                
                isSuccessful = true;
            }
            catch (HttpRequestException)
            {
                isSuccessful = false;
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
