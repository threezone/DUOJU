using DUOJU.Domain.Exceptions;
using DUOJU.Domain.Resources;
using System;
using System.Resources;

namespace DUOJU.Domain.Extensions
{
    public static class LocalizedExtension
    {
        private const string BASICSYSTEMEXCEPTION_NAME = "BasicSystemException";


        public static string ToLocalize(this Enum obj)
        {
            var key = string.Format("{0}_{1}", obj.GetType().Name, obj.ToString());
            var resource = new ResourceManager(typeof(EnumResource));

            return resource.GetString(key) ?? obj.ToString();
        }

        public static string ToLocalize(this BasicSystemException ex)
        {
            var key = string.Format("{0}_{1}", BASICSYSTEMEXCEPTION_NAME, ex.GetType().Name);
            var resource = new ResourceManager(typeof(ExceptionResource));

            return resource.GetString(key) ?? ex.GetType().Name;
        }
    }
}
