//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinemonito.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public long Id { get; set; }
        public long IdClient { get; set; }
        public long IdMovieByRoom { get; set; }
        public int Quantity { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual MoviesByRoom MoviesByRoom { get; set; }
    }
}
