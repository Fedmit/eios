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

        Command _refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }

        public StudentsListViewModel()
        {
            _studentsList = new List<Student>();

            Task.Run(async () =>
            {
                IsBusy = true;
                StudentsList = await PopulateList();
                await UpdateState();
                IsBusy = false;
            });
        }

        async Task<List<Student>> PopulateList()
        {
            var studentsList = await WebApi.Instance.GetStudentsAsync(1);
            return studentsList;
        }

        async Task UpdateState()
        {
            List<Student> students = await WebApi.Instance.GetStudentsAsync(1);

            if (students != null)
            {
                foreach (Student student in students)
                {
                    var obj = StudentsList.FirstOrDefault(x => x.Id == student.Id);
                    if (obj != null) obj.FullName = student.FullName;
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