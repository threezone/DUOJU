using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "District", Lazy=false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class District
    {
        [Id(0, Name = "DistrictId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int DistrictId { get; set; }

        [Property]
        public virtual string DistrictCode { get; set; }

        [Property]
        public virtual string DistrictName { get; set; }

        [Property]
        public virtual string CityCode { get; set; }

        [Property]
        public virtual string PinYinName { get; set; }

        [Property]
        public virtual string PinYinNameAbbr { get; set; }
    }
}
