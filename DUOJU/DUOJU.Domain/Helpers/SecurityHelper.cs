using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DUOJU.Domain.Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string encryptString)
        {
            var bytes = Encoding.Default.GetBytes(encryptString);

            var sha = new SHA1CryptoServiceProvider();
            bytes = sha.ComputeHash(bytes);

            return string.Join("", bytes.Select(i => string.Format("{0:x2}", i)));
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            var aes = new RijndaelManaged();
            aes.Key = UTF8Encoding.UTF8.GetBytes(encryptKey);
            aes.IV = UTF8Encoding.UTF8.GetBytes(encryptKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var encryptDatas = UTF8Encoding.UTF8.GetBytes(encryptString);
            var encryptor = aes.CreateEncryptor();
            var result = encryptor.TransformFinalBlock(encryptDatas, 0, encryptDatas.Length);

            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptString, string decryptKey)
        {
            var aes = new RijndaelManaged();
            aes.Key = UTF8Encoding.UTF8.GetBytes(decryptKey);
            aes.IV = UTF8Encoding.UTF8.GetBytes(decryptKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var decryptDatas = Convert.FromBase64String(decryptString);
            var decryptor = aes.CreateDecryptor();
            var result = decryptor.TransformFinalBlock(decryptDatas, 0, decryptDatas.Length);

            return Encoding.UTF8.GetString(result);
        }
    }
}
