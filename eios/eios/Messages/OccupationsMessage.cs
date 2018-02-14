using eios.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Messages
{
    public class OccupationsMessage
    {
        public bool IsSuccessful { get; set; }

        public List<List<Occupation>> Data { get; set; }
    }
}
