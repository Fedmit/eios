using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.ViewModel
{
    class OccupationsListViewModel : INotifyPropertyChanged
    {
        ContentPage Context { get; set; }

        DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;

                var dateNow = DateTime.Parse((string)App.Current.Properties["DateNow"]);
                if (value != dateNow && CrossConnectivity.Current.IsConnected)
                {
                    IsBusy = true;
                    App.IsTimeTravelMode = true;
                    App.DateNow = value;
                    HandleTaskMessages();
                    MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");
                }
                else
                {
                    App.IsTimeTravelMode = false;
                }

                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(DateStr));
            }
        }

        public string DateStr
        {
            get
            {
                if (Date == new DateTime(2016, 6, 1))
                {
                    return "";
                }
                return Date.ToString("dd/MM/yyyy") + "  ▼";
            }
        }

        string _group;
        public string Group
        {
            get
            {
                if (_group == null)
                {
                    var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
                    _group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;
                    return "";
                }

                return _group + "  ▼";
            }
            set
            {
                _group = value;
                OnPropertyChanged(nameof(Group));
            }
        }


        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        List<Occupation> _occupationsList;
        public List<Occupation> OccupationsList
        {
            get { return _occupationsList; }
            set
            {
                _occupationsList = value;
                OnPropertyChanged(nameof(OccupationsList));
            }
        }

        Command _refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }

        public OccupationsListViewModel(ContentPage context)
        {
            _occupationsList = new List<Occupation>();
            _refreshCommand = new Command(async () => await RefreshList());
            Context = context;

            IsBusy = true;
            if (App.IsLoading)
            {
                HandleTaskMessages();
            }
            else
            {
                Task.Run(async () =>
                {
                    var dateNow = DateTime.Parse((string)App.Current.Properties["DateNow"]);
                    Date = dateNow;

                    var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
                    Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                    await UpdateOccupationsList();
                    IsBusy = false;

                    MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (message.IsSuccessful)
                            {
                                await UpdateState();
                            }
                        });
                    });
                });
            }
        }

        void HandleTaskMessages()
        {
            OccupationsList = new List<Occupation>();
            MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful)
                    {
                        var dateNow = DateTime.Parse((string)App.Current.Properties["DateNow"]);
                        Date = dateNow;

                        var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
                        Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                        await UpdateOccupationsList();

                        MessagingCenter.Send(new StartSyncScheduleStateTaskMessage(), "StartSyncScheduleStateTaskMessage");

                        MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", _message =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (message.IsSuccessful)
                                {
                                    await UpdateState();
                                }
                            });
                        });
                    }
                    else
                    {
                        await Context.DisplayAlert(
                            "Ошибка",
                            "Произошла ошибка при загрузке данных",
                            "ОК");
                    }
                    IsBusy = false;
                });
            });
        }

        async public Task UpdateOccupationsList()
        {
            var occupationList = await PopulateList();
            OccupationsList = occupationList;
        }

        async Task<List<Occupation>> PopulateList()
        {
            return await App.Database.GetOccupations((int)App.Current.Properties["IdGroupCurrent"]);
        }

        async Task RefreshList()
        {
            IsRefreshing = true;
            await UpdateState();
            IsRefreshing = false;
        }

        async Task UpdateState()
        {
            var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
            var occupationsList = await App.Database.GetOccupations(idGroup);
            OccupationsList = occupationsList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
