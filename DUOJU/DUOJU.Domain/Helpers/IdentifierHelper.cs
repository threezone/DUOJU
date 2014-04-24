using System;
using System.Linq;
using System.Text;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Models.Identifier;

namespace DUOJU.Domain.Helpers
{
    public static class IdentifierHelper
    {
        private static string ConvertIdentifierString(IdentifierInfo info)
        {
            var words = new StringBuilder();
            var random = new Random();
            for (int i = 0, max = CommonSettings.IDENTIFIERNO_WORDS.Length; i < CommonSettings.IDENTIFIERNO_RANDOMWORDSLENGTH; ++i)
                words.Append(CommonSettings.IDENTIFIERNO_WORDS[random.Next(0, max)]);

            return string.Format(
                CommonSettings.IDENTIFIERNO_FORMAT,
                info.CreateTime,
                info.ExpiresTime,
                (int)info.Type,
                info.Parameters != null && info.Parameters.Length > 0 ?
                    string.Join(CommonSettings.IDENTIFIERNO_PARAMETERSEPARATOR.ToString(), info.Parameters.Select(p => p.ToString())) :
                    string.Empty,
                words.ToString()
            );
        }

        private static IdentifierInfo ConvertIdentifierInfo(string identifierString)
        {
            var array = identifierString.Split(CommonSettings.IDENTIFIERNO_FIELDSEPARATOR);

            return new IdentifierInfo
            {
                CreateTime = long.Parse(array[0]),
                ExpiresTime = long.Parse(array[1]),
                Type = (IdentifierTypes)Enum.Parse(typeof(IdentifierTypes), array[2]),
                Parameters = string.IsNullOrEmpty(array[3]) ?
                    null :
                    array[3].Split(CommonSettings.IDENTIFIERNO_PARAMETERSEPARATOR),
                RandomWords = array[4]
            };
        }


        public static string GeneratePartyIdentifier(IdentifierInfo info)
        {
            return SecurityHelper.AESEncrypt(ConvertIdentifierString(info), CommonSettings.IDENTIFIERKEY_PARTY);
        }

        public static IdentifierInfo DecryptPartyIdentifier(string identifier)
        {
            return ConvertIdentifierInfo(SecurityHelper.AESDecrypt(identifier, CommonSettings.IDENTIFIERKEY_PARTY));
        }
    }
}
