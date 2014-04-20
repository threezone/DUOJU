using System;

namespace DUOJU.WECHAT.Sys.Attributes
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}