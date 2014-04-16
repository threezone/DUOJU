using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "AuthorityInfo")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class AuthorityInfo
    {
        [Id(0, Name = "AuthorityId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int AuthorityId { get; set; }

        [Property]
        public virtual string AuthorityName { get; set; }

        [Property]
        public virtual int? GroupId { get; set; }

        [Property]
        public virtual int? UserId { get; set; }

        [Property]
        public virtual int ActionId { get; set; }

        [Property]
        public virtual int Sort { get; set; }

        [Property]
        public virtual DateTime? CreateDate { get; set; }

        [Property]
        public virtual int CreateBy { get; set; }

        [Property]
        public virtual DateTime? ModifyDate { get; set; }

        [Property]
        public virtual int? ModifyBy { get; set; }
    }
}
