using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    class Mark
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mark")]
        public string mMark { get; set; }
    }
}
