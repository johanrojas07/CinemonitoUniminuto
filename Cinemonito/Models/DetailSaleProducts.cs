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
    
    public partial class DetailSaleProducts
    {
        public long IdSale { get; set; }
        public long IdProduct { get; set; }
        public int Quantity { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SaleProducts SaleProducts { get; set; }
    }
}
