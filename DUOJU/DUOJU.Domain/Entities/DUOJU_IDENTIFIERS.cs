//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DUOJU.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DUOJU_IDENTIFIERS
    {
        public DUOJU_IDENTIFIERS()
        {
            this.DUOJU_PARTIES = new HashSet<DUOJU_PARTIES>();
            this.DUOJU_IDENTIFIER_SETTINGS = new HashSet<DUOJU_IDENTIFIER_SETTINGS>();
        }
    
        public int IDENTIFIER_ID { get; set; }
        public int IDENTIFIER_TYPE { get; set; }
        public string IDENTIFIER_NO { get; set; }
        public Nullable<System.DateTime> EXPIRES_TIME { get; set; }
        public int STATUS { get; set; }
        public int CREATE_BY { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public int LAST_UPDATE_BY { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual ICollection<DUOJU_PARTIES> DUOJU_PARTIES { get; set; }
        public virtual ICollection<DUOJU_IDENTIFIER_SETTINGS> DUOJU_IDENTIFIER_SETTINGS { get; set; }
    }
}
