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
    
    public partial class DUOJU_IMAGES
    {
        public DUOJU_IMAGES()
        {
            this.DUOJU_SUPPLIER_IMAGES = new HashSet<DUOJU_SUPPLIER_IMAGES>();
        }
    
        public int IMAGE_ID { get; set; }
        public int IMAGECATEGORY_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string URL { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public Nullable<int> SORTORDER { get; set; }
        public int CREATE_BY { get; set; }
        public System.DateTime CREATE_TIME { get; set; }
        public int LAST_UPDATE_BY { get; set; }
        public System.DateTime LAST_UPDATE_TIME { get; set; }
    
        public virtual DUOJU_IMAGECATEGORY DUOJU_IMAGECATEGORY { get; set; }
        public virtual ICollection<DUOJU_SUPPLIER_IMAGES> DUOJU_SUPPLIER_IMAGES { get; set; }
    }
}