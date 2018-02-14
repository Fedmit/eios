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

        DateTime _date = DateTime.MinValue;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                Console.WriteLine("value: " + value.ToString("yyyy-MM-dd"));
                Console.WriteLine("_date: " + _date.ToString("yyyy-MM-dd"));
                Console.WriteLine("App.DateNow: " + App.DateNow.ToString("yyyy-MM-dd"));
                Console.WriteLine("App.DateSelected: " + App.DateSelected.ToString("yyyy-MM-dd"));

                if (_date == value) { return; }
                else if (_date == DateTime.MinValue)
                {
                    _date = value;

                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(DateStr));

                    return;
                }
                if (value != App.DateNow) { App.IsTimeTravelMode = true; }

                _date = value;
                App.DateSelected = value;

                IsBusy = true;
                App.IsLoading = true;
                MessagingCenter.Send(new StartSyncScheduleTaskMessage(), "StartSyncScheduleTaskMessage");

                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(DateStr));
            }
        }

        public string DateStr
        {
            get
            {
                if (Date == DateTime.MinValue)
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
                    var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
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
            
            if (!App.IsLoading)
            {
                Task.Run(async () =>
                {
                    Date = App.DateSelected;

                    var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
                    Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                    await UpdateOccupationsList();
                    IsBusy = false;
                });
            }

            HandleTaskMessages();
        }

        void HandleTaskMessages()
        {
            OccupationsList = new List<Occupation>();

            MessagingCenter.Subscribe<OnDateSyncronizedMessage>(this, "OnDateSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Date = App.DateSelected;
                });
            });

            MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful && !App.IsLoading)
                    {
                        await UpdateState();
                    }
                });
            });

            MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful)
                    {
                        var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
                        Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                        await UpdateOccupationsList();
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
            return await App.Database.GetOccupations((int) App.Current.Properties["IdGroupCurrent"]);
        }

        async Task RefreshList()
        {
            IsRefreshing = true;
            await UpdateState();
            IsRefreshing = false;
        }

        async Task UpdateState()
        {
            var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
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
