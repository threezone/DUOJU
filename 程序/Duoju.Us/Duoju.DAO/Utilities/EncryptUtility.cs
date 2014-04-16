using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Duoju.DAO.Utilities
{
    public class EncryptUtility
    {
        public static string Md5(string str)
        {
            try
            {
                byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(hashvalue).Replace("-","");
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
