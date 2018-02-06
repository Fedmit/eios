using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace eios.Model
{
    [Table("Attendance")]
    public class Attendance
    {
        [JsonProperty("total"), Ignore]
        public int Total { get; set; }

        [JsonProperty("present"), Ignore]
        public int Present { get; set; }
        
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public int id_student { get; set; }
        
        public int id_ocup { get; set; }

        public int id_group { get; set; }
    }
}
