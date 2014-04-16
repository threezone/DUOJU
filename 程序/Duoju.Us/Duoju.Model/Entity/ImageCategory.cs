using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Duoju.Model.Entity
{
    [Class(Table = "ImageCategory", Lazy = false)]
    [Cache(1, Usage = CacheUsage.ReadWrite)]
    public class ImageCategory
    {
        [Id(0, Name = "ImageCategoryId", TypeType = typeof(int), Column = "ImageCategoryId")]
        [Generator(1, Class = "native")]
        public virtual int ImageCategoryId
        { get; set; }

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
    }
}
