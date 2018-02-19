using eios.Data;
using eios.Messages;
using eios.Model;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.ViewModel
{
    public class OccupationsListViewModel : INotifyPropertyChanged
    {
        ContentPage Context { get; set; }

        string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
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
                else if (App.Groups.Count == 1)
                {
                    return _group;
                }
                else
                {
                    return _group + "  ▼";
                }
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

            HandleTaskMessages();

            if (!App.IsScheduleSync)
            {
                Task.Run(async () =>
                {
                    Group = App.Groups.Where(group => group.IdGroup == App.IdGroupCurrent).ToList()[0].Name;
                    Date = App.DateSelected.ToString("dd/MM/yyyy") + "  ▼";

                    await UpdateOccupationsList();
                    await UpdateState();

                    IsBusy = false;
                });
            }
        }

        void HandleTaskMessages()
        {
            OccupationsList = new List<Occupation>();

            MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var marks = await App.Database.GetMarks(App.IdGroupCurrent);

                    if (marks != null)
                    {
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
                        Group = App.Groups.Where(group => group.IdGroup == App.IdGroupCurrent).ToList()[0].Name;
                        Date = App.DateSelected.ToString("dd/MM/yyyy") + "  ▼";

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
            if (occupationList != null)
            {
                OccupationsList = occupationList;
            }
        }

        async Task<List<Occupation>> PopulateList()
        {
            return await App.Database.GetOccupations(App.IdGroupCurrent);
        }

        async Task RefreshList()
        {
            if (!App.IsScheduleSync && CrossConnectivity.Current.IsConnected)
            {
                IsRefreshing = true;
                await UpdateState();
                IsRefreshing = false;
            }
            else
            {
                IsRefreshing = false;
            }
        }

        public async Task UpdateState()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var marksResponse = await WebApi.Instance.GetMarksAsync();
                if (marksResponse != null && marksResponse.Data != null)
                {
                    await App.Database.SetMarks(marksResponse.Data, App.IdGroupCurrent);
                    App.IdOccupNow = marksResponse.IdOccupNow;

                    MessagingCenter.Send(new OnMarksUpdatedMessage(), "OnMarksUpdatedMessage");
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
