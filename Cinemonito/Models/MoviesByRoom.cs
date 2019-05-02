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
    
    public partial class MoviesByRoom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MoviesByRoom()
        {
            this.ChairByMovie = new HashSet<ChairByMovie>();
            this.Ticket = new HashSet<Ticket>();
        }
    
        public long IdMovieByRoom { get; set; }
        public long IdRoom { get; set; }
        public long IdMovie { get; set; }
        public System.DateTime Horary { get; set; }
    
        public virtual Movie Movie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChairByMovie> ChairByMovie { get; set; }
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
