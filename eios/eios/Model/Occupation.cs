using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    class Occupation
    {
        [JsonProperty("id_occup")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
