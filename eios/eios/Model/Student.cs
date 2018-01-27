using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    class Student
    {
        [JsonProperty("id_student")]
        public int Id { get; set; }

        [JsonProperty("fio")]
        public string FullName { get; set; }
    }
}
