using eios.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace eios.ViewModel
{
    public class CompletedOccupationListViewModel : INotifyPropertyChanged
    {
        DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
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
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        int _present;
        public int Present
        {
            get { return _present; }
            set
            {
                _present = value;
                OnPropertyChanged(nameof(Present));
            }
        }

        public CompletedOccupationListViewModel(DateTime date, string nameOccupation, int idOccupation)
        {
            Date = date;
            NameOccupation = nameOccupation;

            Task.Run(async () =>
            {
                var attendance = await WebApi.Instance.GetAttendanceAsync(idOccupation);

                if (attendance != null)
                {
                    Total = attendance.Total;
                    Present = attendance.Present;
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
