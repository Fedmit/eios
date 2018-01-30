using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace eios.Model
{
    class Occupation : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("id_occup")]
        public int IdOccupation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        private string _mark;
        public string Mark
        {
            get { return _mark; }
            set
            {
                _mark = value;
                OnPropertyChanged(nameof(CircleColor));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
