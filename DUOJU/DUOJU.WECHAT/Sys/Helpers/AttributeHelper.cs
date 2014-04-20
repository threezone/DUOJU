using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DUOJU.WECHAT.Sys.Helpers
{
    public static class AttributeHelper
    {
        public static TAttribute GetAttributeOnTypeOrAssembly<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var attribute = type.First<TAttribute>();
            if (attribute == null)
                attribute = type.Assembly.First<TAttribute>();

            return attribute;
        }

        public static TAttribute First<TAttribute>(this ICustomAttributeProvider attributeProvider) where TAttribute : Attribute
        {
            return attributeProvider.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }

        public static DisplayAttribute Copy(this DisplayAttribute attribute)
        {
            if (attribute == null)
                return null;

            var copy = new DisplayAttribute();

            copy.Name = attribute.Name;
            copy.GroupName = attribute.GroupName;
            copy.Description = attribute.Description;
            copy.ResourceType = attribute.ResourceType;
            copy.ShortName = attribute.ShortName;
            copy.Prompt = attribute.Prompt;

            return copy;
        }

        public static bool CanSupplyDisplayName(this DisplayAttribute attribute)
        {
            return attribute != null
                && attribute.ResourceType != null
                && !string.IsNullOrEmpty(attribute.Name);
        }
    }
}