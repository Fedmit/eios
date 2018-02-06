using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace eios.Model
{
    [Table("Students")]
    public class Student : INotifyPropertyChanged
    {
        [JsonProperty("id_student"), PrimaryKey, Column("id_student")]
        public int Id { get; set; }

        [JsonProperty("fio"), Column("fio")]
        public string FullName { get; set; }

        public int id_group { get; set; }
        private bool _isSelected = false;
        [Ignore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IconSource));
            }
        }
        [Ignore]
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
