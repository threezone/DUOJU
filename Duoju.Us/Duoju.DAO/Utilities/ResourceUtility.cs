using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Resource;

namespace Duoju.DAO.Utilities
{
    public static class ResourceUtility
    {
        public static Dictionary<string, string> ErrorMessage(int errorCode)
        {
            var rm = new ResourceManager(typeof(ErrorInfo));
            var dict = new Dictionary<string, string>();
            string key_title = string.Format("Error_{0}_Title", errorCode);
            string value_title = rm.GetString(key_title);
            if (!string.IsNullOrEmpty(value_title))
            {
                dict.Add(key_title, value_title);
            }
            string key_message = string.Format("Error_{0}_Message", errorCode);
            string value_message = rm.GetString(key_message);
            if (!string.IsNullOrEmpty(value_message))
            {
                dict.Add(key_message, value_message);
            }
            return dict;
        }
    }
}
