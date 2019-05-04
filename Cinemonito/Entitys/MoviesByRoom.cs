using System;


namespace Cinemonito.Entitys
{
    public class MoviesByRoom
    {
        public int id { get; set; }
        public int idMovie { get; set; }
        public int idRoom { get; set; }
        public string nameMovie { get; set; }
        public DateTime Horary { get; set; }
    }
}