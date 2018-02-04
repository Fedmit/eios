using eios.Data;
using eios.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace eios.ViewModel
{
    public class CompletedOccupationListViewModel : INotifyPropertyChanged
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

        public CompletedOccupationListViewModel(Occupation occupation)
        {
            Time = occupation.Time;
            NameOccupation = occupation.Name;

            Task.Run(async () =>
            {
                var attendance = await WebApi.Instance.GetAttendanceAsync(occupation.IdOccupation);

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
