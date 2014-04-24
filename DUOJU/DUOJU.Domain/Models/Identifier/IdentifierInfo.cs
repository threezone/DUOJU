using DUOJU.Domain.Enums;

namespace DUOJU.Domain.Models.Identifier
{
    public class IdentifierInfo
    {
        public long CreateTime { get; set; }

        public long ExpiresTime { get; set; }

        public IdentifierTypes Type { get; set; }

        public object[] Parameters { get; set; }

        public string RandomWords { get; set; }
    }
}
