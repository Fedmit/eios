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
        int counter = 0;

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

                if (CrossConnectivity.Current.IsConnected)
                {
                    _date = value;
                    App.DateSelected = value;

                    this.counter++;
                    Console.WriteLine("this.counter: " + this.counter);

                    IsBusy = true;
                    App.IsLoading = true;
                    MessagingCenter.Send(new StartGetScheduleTaskMessage(), "StartGetScheduleTaskMessage");
                }
                else
                {
                    _date = App.DateSelected;
                }

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
                    _group = App.Groups.Where(group => group.IdGroup == App.IdGroupCurrent).ToList()[0].Name;
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
                    var occupations = await App.Database.GetOccupations(App.IdGroupCurrent);
                    Console.WriteLine(occupations.Count);

                    Date = App.DateSelected;

                    Group = App.Groups.Where(group => group.IdGroup == App.IdGroupCurrent).ToList()[0].Name;

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

            MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!App.IsLoading)
                    {
                        var marks = await App.Database.GetMarks(App.IdGroupCurrent);
                        for (int i = 0; i < marks.Count; i++)
                        {
                            OccupationsList[i].IsChecked = marks[i].IsChecked;
                            OccupationsList[i].IsBlocked = marks[i].IsBlocked;
                        }
                    }
                });
            });

            MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (message.IsSuccessful)
                    {
                        if (message.IsFirstTime)
                        {
                            Date = App.DateSelected;
                        }

                        Group = App.Groups.Where(group => group.IdGroup == App.IdGroupCurrent).ToList()[0].Name;

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
            return await App.Database.GetOccupations(App.IdGroupCurrent);
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
            if (!CrossConnectivity.Current.IsConnected)
            {
                var marksResponse = await WebApi.Instance.GetMarksAsync();
                await App.Database.SetMarks(marksResponse.Data, App.IdGroupCurrent);

                App.IdOccupNow = marksResponse.IdOccupNow;

                MessagingCenter.Send(new OnMarksUpdatedMessage(), "OnMarksUpdatedMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
