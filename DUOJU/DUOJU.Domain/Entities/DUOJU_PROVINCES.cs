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
    
    public partial class DUOJU_PROVINCES
    {
        public DUOJU_PROVINCES()
        {
            this.DUOJU_CITIES = new HashSet<DUOJU_CITIES>();
            this.DUOJU_USERS = new HashSet<DUOJU_USERS>();
            this.DUOJU_SUPPLIERS = new HashSet<DUOJU_SUPPLIERS>();
        }
    
        public int PROVINCE_ID { get; set; }
        public int COUNTRY_ID { get; set; }
        public string PROVINCE_NAME { get; set; }
        public string PROVINCE_NAME_CN { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual ICollection<DUOJU_CITIES> DUOJU_CITIES { get; set; }
        public virtual DUOJU_COUNTRIES DUOJU_COUNTRIES { get; set; }
        public virtual ICollection<DUOJU_USERS> DUOJU_USERS { get; set; }
        public virtual ICollection<DUOJU_SUPPLIERS> DUOJU_SUPPLIERS { get; set; }
    }
}
