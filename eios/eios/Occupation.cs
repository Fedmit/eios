using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eios
{
    public class Occupation : INotifyPropertyChanged
    {
        private string id;
        private string name;
        private string aud;
        private string time;

        [JsonProperty("id_occup")]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [JsonProperty("aud")]
        public string Aud
        {
            get { return aud; }
            set
            {
                aud = value;
                NotifyPropertyChanged("Aud");
            }
        }

        [JsonProperty("time")]
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged("Time");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
