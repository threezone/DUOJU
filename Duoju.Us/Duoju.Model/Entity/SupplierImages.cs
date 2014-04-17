using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "SupplierImages", Lazy = false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class SupplierImages
    {
        [Id(0, Name = "SupplierImageId", TypeType = typeof(int), Column = "SupplierImageId")]
        [Generator(1, Class = "native")]
        public virtual int SupplierImageId
        { get; set; }

        [Property]
        public virtual int SupplierId 
        { 
            get; 
            set; 
        }

        [Property]
        public virtual int ImageId 
        { 
            get; 
            set; 
        }

        [Property]
        public virtual DateTime CreatedDate
        {
            get;
            set;
        }
    }
}
