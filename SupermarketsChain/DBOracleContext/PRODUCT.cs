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
    
    public partial class PRODUCT
    {
        public PRODUCT()
        {
            this.SUPERMARKETS = new HashSet<SUPERMARKET>();
        }
    
        public int ID { get; set; }
        public int VENDORID { get; set; }
        public string NAME { get; set; }
        public int MEASUREID { get; set; }
        public int PRICE { get; set; }
    
        public virtual MEASURE MEASURE { get; set; }
        public virtual VENDOR VENDOR { get; set; }
        public virtual ICollection<SUPERMARKET> SUPERMARKETS { get; set; }
    }
}
