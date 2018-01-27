using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    class Group
    {
        [JsonProperty("id_student")]
        public int IdStudent { get; set; }

        [JsonProperty("id_group")]
        public int IdGroup { get; set; }

        [JsonProperty("fio")]
        public string Fio { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
