using eios.Data;
using eios.Messages;
using eios.Model;
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

        string _date = "lol";
         public string Date
         {
             get { return _date + "  ▼"; }
             set
             {
                _date = value;
                OnPropertyChanged(nameof(Date));
             }
         }

         string _group = "lol";
         public string Group
         {
             get { return _group + "  ▼"; }
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

            IsBusy = true;
            if (App.IsLoading)
            {
                OccupationsList = new List<Occupation>();
                MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message => {
                    Device.BeginInvokeOnMainThread(async () => {
                        if (message.IsSuccessful)
                        {
                            var occupationList = await PopulateList();
                            OccupationsList = occupationList;

                            MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", _message => {
                                Device.BeginInvokeOnMainThread(async () => {
                                    if (message.IsSuccessful)
                                    {
                                        await UpdateState();
                                    }
                                });
                            });
                        }
                        else
                        {
                            await context.DisplayAlert(
                                "Ошибка",
                                "Произошла ошибка при загрузке данных",
                                "ОК");
                        }
                        IsBusy = false;
                    });
                });
            }
            else
            {
                Task.Run(async () =>
                {
                    var occupationList = await PopulateList();
                    OccupationsList = occupationList;
                    IsBusy = false;

                    MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message => {
                        Device.BeginInvokeOnMainThread(async () => {
                            if (message.IsSuccessful)
                            {
                                await UpdateState();
                            }
                        });
                    });
                });
            }
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
