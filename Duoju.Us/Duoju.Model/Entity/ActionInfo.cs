using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "ActionInfo")]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class ActionInfo
    {
        [Id(0, Name = "ActionId", TypeType = typeof(int))]
        [Generator(1, Class = "native")]
        public virtual int ActionId { get; set; }

        [Property]
        public virtual string ActionName { get; set; }

        [Property]
        public virtual int ParentId { get; set; }

        [Property]
        public virtual string UrlPath { get; set; }

        [Property]
        public virtual DateTime? CreateDate { get; set; }

        [Property]
        public virtual int CreateBy { get; set; }

        [Property]
        public virtual DateTime? ModifyDate { get; set; }

        [Property]
        public virtual int? ModifyBy { get; set; }

        [Property]
        public virtual string Icon { get; set; }
    }
}
