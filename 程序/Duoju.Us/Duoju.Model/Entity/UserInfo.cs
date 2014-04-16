using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "UserInfo", Lazy=false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class UserInfo
    {
        [Id(0, Name = "UserId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int UserId { get; set; }

        [Property]
        public virtual string OpenId { get; set; }

        [Property]
        public virtual int Sex { get; set; }

        [Property]
        public virtual string Email { get; set; }

        [Property]
        public virtual DateTime? CreatedDate { get; set; }

        [Property]
        public virtual string NickName { get; set; }

        [Property]
        public virtual string Country { get; set; }

        [Property]
        public virtual string Province { get; set; }

        [Property]
        public virtual string City { get; set; }

        [Property]
        public virtual string HeadImgUrl { get; set; }
    }
}
