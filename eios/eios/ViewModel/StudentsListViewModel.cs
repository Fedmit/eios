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
    class StudentsListViewModel : INotifyPropertyChanged
    {
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

        List<Student> _studentsList;
        public List<Student> StudentsList
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
            _studentsList = new List<Student>();

            Task.Run(async () =>
            {
                IsBusy = true;
                StudentsList = await PopulateList();
                IsBusy = false;
            });
        }

        async Task<List<Student>> PopulateList()
        {
            var studentsList = await WebApi.Instance.GetStudentsAsync();
            return studentsList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}