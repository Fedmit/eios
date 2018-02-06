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
        string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        string _nameOccupation;
        public string NameOccupation
        {
            get { return _nameOccupation; }
            set
            {
                _nameOccupation = value;
                OnPropertyChanged(nameof(NameOccupation));
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

        int _absentTotal;
        public int AbsentTotal
        {
            get
            {
                if (StudentsList != null)
                {
                    return StudentsList.FindAll(s => s.IsAbsent.Equals(true)).Count;
                }
                return 0;
            }
            set
            {
                _absentTotal = value;
                OnPropertyChanged(nameof(AbsentTotal));
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
                    OnPropertyChanged(nameof(AbsentTotal));
                }
            }
        }

        public CompletedOccupationListViewModel(Occupation occupation)
        {
            Time = occupation.Time;
            NameOccupation = occupation.Name;

            Task.Run(async () =>
            {
                IsBusy = true;

                //await Task.Delay(2000);

                //var studentList = new List<StudentAttendance>();
                //for (int i = 0; i < 10; i++)
                //{
                //    studentList.Add(new StudentAttendance()
                //    {
                //        Id = i,
                //        FullName = i + " Федоров Дмитрий Алексеевич",
                //        IsAbsent = i > 7
                //    });
                //}

                //var attendance = await WebApi.Instance.GetAttendanceAsync(occupation.IdOccupation);

                //StudentsList = studentList.OrderByDescending(student => student.IsAbsent).ToList();

                IsBusy = false;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
