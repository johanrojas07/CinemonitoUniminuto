using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinemonito.Entitys
{
    public class ClientEntity
    {
        public int? id { get; set; }
        public string nombre { get; set; }
        public string identificacion { get; set; }
        public int totalPoints { get; set; }
    }

}