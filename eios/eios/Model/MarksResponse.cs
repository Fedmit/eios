using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    public class MarksResponse
    {
        [JsonProperty("id_occup_now")]
        public int IdOccupNow { get; set; }

        [JsonProperty("data")]
        public List<Mark> Data { get; set; }
    }
}
