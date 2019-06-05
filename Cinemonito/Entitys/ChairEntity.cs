using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinemonito.Entitys
{
    public class ChairEntity
    {
        public int Id { get; set; }
        public int idMovieByRoom { get; set; }
        public int idChair { get; set; }
        public bool isAvalible { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int idTypeChair { get; set; }
        public int price { get; set; }
        public bool? isNotAvalibleLocal { get; set; }
}

}