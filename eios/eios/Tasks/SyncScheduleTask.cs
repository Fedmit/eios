using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
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
                if (CrossConnectivity.Current.IsConnected)
                {
                    DateTime lastDate = new DateTime();
                    DateTime dateNow = await WebApi.Instance.GetDateAsync();

                    lastDate = App.DateNow;

                    App.Current.Properties["DateNow"] = dateNow.ToString("yyyy-MM-dd");
                    await App.Current.SavePropertiesAsync();

                    App.DateSelected = App.DateNow;

                    if (lastDate == DateTime.MinValue || lastDate != App.DateNow)
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
