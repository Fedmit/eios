using eios;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace eios.Model
{
    public class Occupation : INotifyPropertyChanged
    {
        [JsonProperty("id_occup")]
        public int IdOccupation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("id_lesson")]
        public string IdLesson { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        public int IdGroup { get; set; }

        private string _mark;
        public string Mark
        {
            get { return _mark; }
            set
            {
                _mark = value;
                OnPropertyChanged(nameof(CircleColor));
                OnPropertyChanged(nameof(TargetType));
            }
        }

        public string CircleColor
        {
            get
            {
                switch (Mark)
                {
                    case "was_no": return "#f7636c";
                    case "was_attend": return "#e0e0e0";
                    case "is_no": return "#acd94e";
                    case "is_attend": return "#e0e0e0";
                    case "will": return "#77aad9";
                    default: return "#77aad9";
                }
            }
        }

        public Type TargetType
        {
            get
            {
                switch (Mark)
                {
                    case "was_no": return null;
                    case "was_attend": return typeof(CompletedOccupationPage);
                    case "is_no": return null;
                    case "is_attend": return typeof(CompletedOccupationPage);
                    case "will": return null;
                    default: return null;
                }
            }
        }

        public string Time
        {
            get
            {
                switch (IdOccupation)
                {
                    case 1: return "8:00";
                    case 2: return "9:50";
                    case 3: return "11:40";
                    case 4: return "13:45";
                    case 5: return "15:35";
                    case 6: return "17:25";
                    case 7: return "19:10";
                    case 8: return "20:55";
                    default: return "-";
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
