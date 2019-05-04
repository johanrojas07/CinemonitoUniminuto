using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinemonito.Entitys
{
    public class EmployeesEntity
    {
        public string id { get; set; }
        public string identification { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string contractDateInit { get; set; }
        public string contractDateEnd { get; set; }
        public string salary { get; set; }
        public string headquarterName { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }

}