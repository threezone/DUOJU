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
    
    public partial class DUOJU_PARTY_PARTICIPANTS
    {
        public int PARTY_PARTICIPANT_ID { get; set; }
        public int PARTY_ID { get; set; }
        public int PARTICIPANT_ID { get; set; }
        public System.DateTime PARTICIPATE_TIME { get; set; }
        public int STATUS { get; set; }
        public int CREATE_BY { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public int LAST_UPDATE_BY { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual DUOJU_PARTIES DUOJU_PARTIES { get; set; }
        public virtual DUOJU_USERS DUOJU_USERS { get; set; }
    }
}
