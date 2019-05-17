using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinemonito.Entitys
{
    public class MoviesByRoom
    {
        public int? id { get; set; }
        public int? idMovie { get; set; }
        public int? idRoom { get; set; }
        public string nameMovie { get; set; }
        public string nameRoom { get; set; }
        public DateTime horary { get; set; }
    }

}