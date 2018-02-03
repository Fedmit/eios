using eios.Data;
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

        public OccupationsListViewModel()
        {
            _occupationsList = new List<Occupation>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                var occupationsList = await PopulateList();
                await App.Database.CreateTable();
                foreach (Occupation ocup in occupationsList)
                {
                    await App.Database.SaveItem(ocup);
                }
                OccupationsList = await App.Database.GetItemsAsync(1);
                await UpdateState();
                IsBusy = false;
            });
        }

        async Task<List<Occupation>> PopulateList()
        {
            var occupationsList = await WebApi.Instance.GetOccupationsAsync(1);
            
            //App.Date = await WebApi.Instance.GetDateAsync();
            //var occupationsList = await WebApi.Instance.GetOccupationsAsync();
            return occupationsList;
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
                    var obj = OccupationsList.FirstOrDefault(x => x.Id == mark.Id);
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
