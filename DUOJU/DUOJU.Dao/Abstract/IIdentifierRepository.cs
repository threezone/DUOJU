using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Abstract
{
    public interface IIdentifierRepository
    {
        int SaveChanges();

        void AddIdentifier(DUOJU_IDENTIFIERS identifier);

        bool IsIdentifierNOUnique(string identifierNO);

        DUOJU_IDENTIFIERS GetIdentifierById(int identifierId);

        DUOJU_IDENTIFIERS GetIdentifierByIdentifierNO(string identifierNO);
    }
}
