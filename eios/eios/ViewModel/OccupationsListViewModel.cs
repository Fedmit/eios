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
                MessagingCenter.Subscribe<OnScheduleSyncronizedMessage>(this, "OnScheduleSyncronizedMessage", message => {
                    Device.BeginInvokeOnMainThread(async () => {
                        if (message.IsSuccessful)
                        {
                            OccupationsList = await PopulateList();
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
                    OccupationsList = await PopulateList();
                    IsBusy = false;
                });
            }

            MessagingCenter.Subscribe<OnMarksUpdatedMessage>(this, "OnMarksUpdatedMessage", message => {
                Device.BeginInvokeOnMainThread(async () => {
                    if (message.IsSuccessful)
                    {
                        await UpdateState();
                    }
                });
            });
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
            List<Mark> marks = await WebApi.Instance.GetMarksAsync();

            if (marks != null)
            {
                foreach (Mark mark in marks)
                {
                    var obj = OccupationsList.FirstOrDefault(x => x.IdOccupation == mark.Id);
                    if (obj != null) obj.Mark = mark.mMark;
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
