using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "City", Lazy=false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class City
    {
        [Id(0, Name = "CityId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int CityId { get; set; }

        [Property]
        public virtual string CityCode { get; set; }

        [Property]
        public virtual string CityName { get; set; }

        [Property]
        public virtual string ProvinceCode { get; set; }

        [Property]
        public virtual string PinYinName { get; set; }

        [Property]
        public virtual string PinYinNameAbbr { get; set; }
    }
}
