using eios;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace eios.Model
{
    [Table("Occupations")]
    public class Occupation : INotifyPropertyChanged
    {
        [JsonProperty("id"), PrimaryKey, Column("_id")]
        public int Id { get; set; }

        [JsonProperty("id_occup"), Column("IdOccupation")]
        public int IdOccupation { get; set; }

        [Column("IdGroup")]
        public int IdGroup { get; set; }

        [JsonProperty("name"), Column("Name")]
        public string Name { get; set; }

        [JsonProperty("aud"), Column("Aud")]
        public string Aud { get; set; }

        [JsonProperty("time"), Column("IdTime")]
        public DateTime Time { get; set; }

        private string _mark;

       // [Column("Mark")]
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
        [Ignore]
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
        [Ignore]
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
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
