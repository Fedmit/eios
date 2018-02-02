using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    public class Attendance
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("present")]
        public int Present { get; set; }
    }
}
