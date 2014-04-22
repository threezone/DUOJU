//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DUOJU.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DUOJU_PARTIES
    {
        public DUOJU_PARTIES()
        {
            this.DUOJU_PARTY_PARTICIPANTS = new HashSet<DUOJU_PARTY_PARTICIPANTS>();
            this.DUOJU_PARTY_COMMENTS = new HashSet<DUOJU_PARTY_COMMENTS>();
        }
    
        public int PARTY_ID { get; set; }
        public int SUPPLIER_ID { get; set; }
        public int INITIATOR_ID { get; set; }
        public System.DateTime HOLD_DATE { get; set; }
        public int HOLD_TIME { get; set; }
        public string DESCRIPTION { get; set; }
        public int MIN_INTO_FORCE { get; set; }
        public Nullable<int> MAX_INTO_FORCE { get; set; }
        public string CONSUMPTION_VOUCHER_NO { get; set; }
        public int STATUS { get; set; }
        public int CREATE_BY { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public int LAST_UPDATE_BY { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual DUOJU_USERS DUOJU_USERS { get; set; }
        public virtual ICollection<DUOJU_PARTY_PARTICIPANTS> DUOJU_PARTY_PARTICIPANTS { get; set; }
        public virtual ICollection<DUOJU_PARTY_COMMENTS> DUOJU_PARTY_COMMENTS { get; set; }
    }
}
