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
    
    public partial class DUOJU_SUPPLIERINFO
    {
        public int SUPPLIERINFO_ID { get; set; }
        public int SUPPLIER_ID { get; set; }
        public string MAINTITLE { get; set; }
        public string SUBTITLE { get; set; }
        public Nullable<int> VIEWCOUNT { get; set; }
        public Nullable<int> PARTYCOUNT { get; set; }
        public Nullable<int> FAVOURCOUNT { get; set; }
        public string CONTENT { get; set; }
        public int CREATE_BY { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public int LAST_UPDATE_BY { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual DUOJU_SUPPLIER DUOJU_SUPPLIER { get; set; }
    }
}
