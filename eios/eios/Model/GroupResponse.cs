using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    public class GroupResponse
    {
        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("data")]
        public List<Group> Data { get; set; }
    }
}
