using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eios.Model
{
    class SelectedStudent
    {
        [JsonProperty("id_student")]
        public int Id { get; set; }
    }
}
