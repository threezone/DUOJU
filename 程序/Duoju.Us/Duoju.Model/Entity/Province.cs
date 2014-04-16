using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "Province", Lazy=false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class Province
    {
        [Id(0, Name = "ProvinceId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int ProvinceId { get; set; }

        [Property]
        public virtual string ProvinceCode { get; set; }

        [Property]
        public virtual string ProvinceName { get; set; }

        [Property]
        public virtual string PinYinName { get; set; }

        [Property]
        public virtual string PinYinNameAbbr { get; set; }

        [Property]
        public virtual int? OrderSort { get; set; }
    }
}
