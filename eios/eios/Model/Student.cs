using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eios.Model
{
    class Student : INotifyPropertyChanged
    {
        [JsonProperty("id_student")]
        public int Id { get; set; }

        [JsonProperty("fio")]
        public string FullName { get; set; }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IconSource));
            }
        }

        public string IconSource
        {
            get
            {
                if (IsSelected)
                {
                    return "check.png";
                }
                else
                {
                    return "uncheck.png";
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
