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
    
    public partial class DUOJU_CITIES
    {
        public DUOJU_CITIES()
        {
            this.DUOJU_USERS = new HashSet<DUOJU_USERS>();
        }
    
        public int CITY_ID { get; set; }
        public Nullable<int> COUNTRY_ID { get; set; }
        public Nullable<int> PROVINCE_ID { get; set; }
        public string CITY_NAME { get; set; }
        public string CITY_NAME_CN { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual DUOJU_COUNTRIES DUOJU_COUNTRIES { get; set; }
        public virtual DUOJU_PROVINCES DUOJU_PROVINCES { get; set; }
        public virtual ICollection<DUOJU_USERS> DUOJU_USERS { get; set; }
    }
}
