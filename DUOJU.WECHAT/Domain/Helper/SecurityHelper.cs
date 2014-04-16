using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helper
{
    public static class SecurityHelper
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="EncryptString"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string EncryptString)
        {
            var bytes = Encoding.Default.GetBytes(EncryptString);

            var iSHA = new SHA1CryptoServiceProvider();
            bytes = iSHA.ComputeHash(bytes);

            return string.Join("", bytes.Select(i => string.Format("{0:x2}", i)));
        }
    }
}
