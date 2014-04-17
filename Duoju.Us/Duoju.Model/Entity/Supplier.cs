using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "Supplier", Lazy = false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class Supplier
    {
        [Id(0, Name = "SupplierId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int SupplierId { get; set; }

        [OneToOne(Name = "SupplierInfo", ClassType = typeof(SupplierInfo),
            PropertyRef = "SupplierId", Cascade = "All", Fetch = FetchMode.Join)]
        public virtual SupplierInfo SupplierInfo
        { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Address { get; set; }

        [Property]
        public virtual string Telphone { get; set; }

        [Property]
        public virtual DateTime? CreatedDate { get; set; }

        [Property]
        public virtual string Fax { get; set; }

        [Property]
        public virtual string Email { get; set; }

        [Property]
        public virtual string QQ { get; set; }

        [Property]
        public virtual string WebSite { get; set; }

        [Property]
        public virtual decimal Longitude { get; set; }

        [Property]
        public virtual decimal Latitude { get; set; }
    }
}
