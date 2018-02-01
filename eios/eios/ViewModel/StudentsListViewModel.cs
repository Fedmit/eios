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
        DateTime _occupationTime;
        public DateTime OccupationTime
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
                _studentsList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }

        public StudentsListViewModel(DateTime time, string name)
        {
            OccupationTime = time;
            OccupationName = name;
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
            var studentsList = await WebApi.Instance.GetStudentsAsync(1);
            return studentsList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}