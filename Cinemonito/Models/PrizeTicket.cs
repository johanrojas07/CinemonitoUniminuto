//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinemonito.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PrizeTicket
    {
        public long IdClient { get; set; }
        public int Points { get; set; }
        public System.DateTime DatePrize { get; set; }
        public bool IsAvailable { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
