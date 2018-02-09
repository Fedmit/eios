using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace eios.Model
{
    [Table("Groups")]
    public class Group
    {
        [JsonProperty("id_group"), PrimaryKey, Column("id_group")]
        public int IdGroup { get; set; }

        [JsonProperty("name"), Column("group_name")]
        public string Name { get; set; }
    }
}
