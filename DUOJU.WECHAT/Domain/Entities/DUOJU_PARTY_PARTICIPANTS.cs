//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Entities
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
