using eios.Data;
using eios.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return _occupationName; }
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

                StudentsList = await PopulateList();

                IsBusy = false;
            });
        }

        async Task<List<StudentAttendance>> PopulateList()
        {
            var idGroup = (int)App.Current.Properties["IdGroupCurrent"];
            var attendanceList = await App.Database.GetAttendance(Occupation.IdOccupation, idGroup);

            if(attendanceList == null)
            {
                Console.WriteLine("Отмеченных студентов нет лол");
            }

            return attendanceList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
