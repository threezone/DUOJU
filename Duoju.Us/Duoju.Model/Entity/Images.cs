using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "Images", Lazy = false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class Images
    {
        [Id(0, Name = "ImageId", TypeType = typeof(int), Column = "ImageId")]
        [Generator(1, Class = "native")]
        public virtual int ImageId
        {
            get;
            set;
        }

        [Property]
        public virtual int ImageCategoryId
        { get; set; }

        [ManyToOne(Column = "ImageCategoryId", Insert = false, Update = false)]
        public virtual ImageCategory ImageCategory
        {
            get;
            set;
        }

        [Property]
        public virtual int SupplierId
        {
            get;
            set;
        }

        [Property]
        public virtual string Url
        {
            get;
            set;
        }

        [Property]
        public virtual string Description
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

        [Property]
        public virtual int? SortOrder
        {
            get;
            set;
        }      
    }
}
