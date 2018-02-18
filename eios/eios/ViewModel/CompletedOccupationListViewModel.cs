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
    class CompletedOccupationListViewModel : INotifyPropertyChanged
    {
        public Occupation Occupation { get; set; }

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

        int _total;
        public int Total
        {
            get
            {
                if (StudentsList != null)
                {
                    return StudentsList.Count;
                }
                return 0;
            }
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        public int PresentTotal
        {
            get
            {
                if (StudentsList != null)
                {
                    return StudentsList.FindAll(s => s.IsAbsent.Equals(false)).Count;
                }
                return 0;
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

        List<StudentAttendance> _studentsList;
        public List<StudentAttendance> StudentsList
        {
            get { return _studentsList; }
            set
            {
                if (value != null)
                {
                    _studentsList = value;
                    OnPropertyChanged(nameof(StudentsList));
                    OnPropertyChanged(nameof(Total));
                    OnPropertyChanged(nameof(PresentTotal));
                }
            }
        }

        public CompletedOccupationListViewModel(Occupation occupation)
        {
            Occupation = occupation;

            OccupationTime = Occupation.Time;
            OccupationName = Occupation.Name;
            OccupationAud = Occupation.Aud;

            Task.Run(async () =>
            {
                IsBusy = true;

                if (!App.IsAttendanceSync)
                {
                    StudentsList = await PopulateList();
                    IsBusy = false;
                }
                else
                {
                    MessagingCenter.Subscribe<OnAttendanceSyncronizedMessage>(this, "OnAttendanceSyncronizedMessage", message =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            StudentsList = await PopulateList();
                            IsBusy = false;

                            MessagingCenter.Unsubscribe<OnAttendanceSyncronizedMessage>(this, "OnAttendanceSyncronizedMessage");
                        });
                    });
                }
            });
        }

        async Task<List<StudentAttendance>> PopulateList()
        {
            var studentsList = await App.Database.GetAttendance(Occupation.IdOccupation, App.IdGroupCurrent);
            return studentsList.OrderBy(e => e.IsAbsent ? 0 : 1).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
