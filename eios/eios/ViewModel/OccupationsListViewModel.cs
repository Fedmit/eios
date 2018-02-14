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
    public class OccupationsListViewModel : INotifyPropertyChanged
    {
        ContentPage Context { get; set; }

        DateTime _date = DateTime.MinValue;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date == value) { return; }
                else if (_date == DateTime.MinValue)
                {
                    _date = value;

                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(DateStr));

                    return;
                }

                _date = value;
                App.DateSelected = value;

                IsBusy = true;

                App.IsTimeTravelMode = true;
                App.IsLoading = true;
                MessagingCenter.Send(new StartGetScheduleTaskMessage(), "StartGetScheduleTaskMessage");

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
                    App.DateSelected = App.DateNow;
                    Date = App.DateSelected;

                    var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
                    Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                    await UpdateOccupationsList();
                    await UpdateState();

                    IsBusy = false;
                });
            }

            HandleTaskMessages();
        }

        void HandleTaskMessages()
        {
            OccupationsList = new List<Occupation>();

            MessagingCenter.Subscribe<OccupationsMessage>(this, "OccupationsMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful)
                    {
                        var occupationsList = message.Data;
                        OccupationsList = occupationsList;
                        await UpdateState();

                        IsBusy = false;
                    }
                });
            });

            MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful)
                    {
                        App.DateSelected = App.DateNow;
                        Date = App.DateSelected;

                        var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
                        Group = App.Groups.Where(group => group.IdGroup == idGroup).ToList()[0].Name;

                        await UpdateOccupationsList();
                        await UpdateState();
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
            if (!App.IsLoading)
            {
                IsRefreshing = true;
                await UpdateState();
                IsRefreshing = false;
            }
        }

        public async Task UpdateState()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var marksResponse = await WebApi.Instance.GetMarksAsync();
                var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
                if (!App.IsTimeTravelMode)
                {
                    await App.Database.SetMarks(marksResponse.Data, idGroup);
                    App.IdOccupNow = marksResponse.IdOccupNow;

                    var occupationsList = await App.Database.GetOccupations(idGroup);
                    OccupationsList = occupationsList;
                }
                else
                {
                    App.IdOccupNow = marksResponse.IdOccupNow;
                    var occupationsList = OccupationsList;
                    var marks = marksResponse.Data;
                    for (int i = 0; i < marksResponse.Data.Count; i++)
                    {
                        occupationsList[i].IsChecked = marks[i].Checked;
                        occupationsList[i].IsBlocked = marks[i].Blocked;
                    }
                    OccupationsList = occupationsList;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
