using System;
using System.Collections.Generic;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;

namespace DUOJU.Service.Abstract
{
    public interface IIdentifierService
    {
        DUOJU_IDENTIFIERS GenerateIdentifier(IdentifierTypes type, DateTime expiresTime, IList<KeyValuePair<string, string>> settings, int operatorId);

        bool VerifyIdentifier(string identifierNO, int operatorId);
    }
}
