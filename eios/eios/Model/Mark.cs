using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    public class Mark
    {
        [JsonProperty("id_occup")]
        public int IdOccupation { get; set; }

        [JsonProperty("checked")]
        public bool IsChecked { get; set; }

        [JsonProperty("blocked")]
        public bool IsBlocked { get; set; }
    }
}
