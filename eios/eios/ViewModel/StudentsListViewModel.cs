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
    class StudentsListViewModel : INotifyPropertyChanged
    {
        bool _isReadyToMark = false;
        public bool IsReadyToMark
        {
            get { return _isReadyToMark; }
            set
            {
                _isReadyToMark = value;
                OnPropertyChanged(nameof(IsReadyToMark));
            }
        }

        string _occupationTime;
        public string OccupationTime
        {
            get { return _occupationTime; }
            set
            {
                _occupationTime = value;
                OnPropertyChanged(nameof(OccupationTime));
            }
        }

        string _occupationName;
        public string OccupationName
        {
            get { return _occupationName; }
            set
            {
                _occupationName = value;
                OnPropertyChanged(nameof(OccupationName));
            }
        }
        string _occupationAud;
        public string OccupationAud
        {
            get { return _occupationAud; }
            set
            {
                _occupationAud = value;
                OnPropertyChanged(nameof(OccupationAud));
            }
        }

        public int Total
        {
            get
            {
                return StudentsList.Count;
            }
        }

        int _onSite;
        public int OnSite
        {
            get { return _onSite; }
            set
            {
                _onSite = value;
                OnPropertyChanged(nameof(OnSite));
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

        List<StudentSelect> _studentsList;
        public List<StudentSelect> StudentsList
        {
            get { return _studentsList; }
            set
            {
                if (value != null)
                {
                    _studentsList = value;
                    OnPropertyChanged(nameof(StudentsList));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public StudentsListViewModel(Occupation occupation)
        {
            OccupationTime = occupation.Time;
            OccupationName = occupation.Name;
            OccupationAud = occupation.Aud;
            _studentsList = new List<StudentSelect>();

            if (!App.IsAttendanceSync)
            {
                IsReadyToMark = true;
            }
            else
            {
                MessagingCenter.Subscribe<OnAttendanceSyncronizedMessage>(this, "OnAttendanceSyncronizedMessage", message =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsReadyToMark = true;

                        MessagingCenter.Unsubscribe<OnAttendanceSyncronizedMessage>(this, "OnAttendanceSyncronizedMessage");
                    });
                });
            }

            Task.Run(async () =>
            {
                IsBusy = true;
                StudentsList = await PopulateList();
                OnSite = StudentsList.FindAll(s => s.IsSelected.Equals(false)).Count;
                IsBusy = false;
            });
        }

        async Task<List<StudentSelect>> PopulateList()
        {
            return await App.Database.GetStudents(App.IdGroupCurrent);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}