using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "SupplierInfo", Lazy = false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class SupplierInfo
    {
        [Id(0, Name = "SupplierInfoId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int SupplierInfoId { get; set; }

        [Property]
        public virtual int SupplierId { get; set; }

        [Property]
        public virtual string MainTitle { get; set; }

        [Property]
        public virtual string SubTitle { get; set; }

        [Property]
        public virtual int? ViewCount { get; set; }

        [Property]
        public virtual int? PartyCount { get; set; }

        [Property]
        public virtual int? FavourCount { get; set; }

        [Property]
        public virtual string Content { get; set; }

        [Property]
        public virtual DateTime? CreatedDate { get; set; }

        [Property]
        public virtual DateTime? LastUpdateTime { get; set; }
    }
}
