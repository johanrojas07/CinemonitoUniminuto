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
    
    public partial class Employee
    {
        public long Id { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public System.DateTime ContractDate { get; set; }
        public decimal Salary { get; set; }
        public long IdHeadquarter { get; set; }
        public long IdPosition { get; set; }
    
        public virtual Headquarters Headquarters { get; set; }
        public virtual Position Position { get; set; }
    }
}
