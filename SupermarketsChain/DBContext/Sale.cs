//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SupermarketsChain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        public int Id { get; set; }
        public Nullable<int> SupermarketId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public System.DateTime OrderedOn { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Supermarket Supermarket { get; set; }
    }
}
