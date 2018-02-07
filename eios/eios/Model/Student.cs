using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace eios.Model
{
    [Table("Students")]
    public class Student
    {
        [JsonProperty("id_student"), PrimaryKey, Column("id_student")]
        public int Id { get; set; }

        [JsonProperty("fullname"), Column("fullname")]
        public string FullName { get; set; }

        public int id_group { get; set; }
    }

    public class StudentAttendance : Student
    {
        public bool IsAbsent { get; set; }

        public string Color
        {
            get
            {
                if (IsAbsent)
                {
                    return "#ffc9c9";
                }
                return "Transparent";
            }
        }
    }

    public class StudentSelect : Student, INotifyPropertyChanged
    {
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

    [Table("Attendance")]
    public class StudentAbsent
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        [Column("id_student")]
        public int Id { get; set; }

        [Column("id_ocup")]
        public int IdOccupation { get; set; }

        [Column("id_group")]
        public int IdGroup { get; set; }
    }
}
