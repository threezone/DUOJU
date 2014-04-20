using System;

namespace DUOJU.WECHAT.Sys.Helpers
{
    public static class ReflectionHelper
    {
        public static bool PropertyExists(this Type type, string propertyName)
        {
            if (type == null || propertyName == null)
                return false;

            return type.GetProperty(propertyName) != null;
        }
    }
}