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
    
    public partial class ChairByMovie
    {
        public long IdMovieByRoom { get; set; }
        public long IdChair { get; set; }
        public bool IsAvailable { get; set; }
    
        public virtual Chair Chair { get; set; }
        public virtual MoviesByRoom MoviesByRoom { get; set; }
    }
}
