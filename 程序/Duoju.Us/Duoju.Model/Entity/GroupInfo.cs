using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "GroupInfo")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class GroupInfo
    {
        [Id(0, Name = "GroupId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int GroupId { get; set; }

        [Property]
        public virtual string GroupName { get; set; }

        [Property]
        public virtual string Discription { get; set; }

        [Property]
        public virtual string UrgentPhone { get; set; }

        [Property]
        public virtual int? ManagerUserId { get; set; }

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
