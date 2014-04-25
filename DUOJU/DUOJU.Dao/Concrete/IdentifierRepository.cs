using System.Linq;
using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Concrete
{
    public class IdentifierRepository : Repository, IIdentifierRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public void AddIdentifier(DUOJU_IDENTIFIERS identifier)
        {
            DBEntities.DUOJU_IDENTIFIERS.Add(identifier);
        }

        public bool IsIdentifierNOUnique(string identifierNO)
        {
            return DBEntities.DUOJU_IDENTIFIERS.Count(i => i.IDENTIFIER_NO == identifierNO) == 0;
        }

        public DUOJU_IDENTIFIERS GetIdentifierById(int identifierId)
        {
            return DBEntities.DUOJU_IDENTIFIERS.SingleOrDefault(p => p.IDENTIFIER_ID == identifierId);
        }

        public DUOJU_IDENTIFIERS GetIdentifierByIdentifierNO(string identifierNO)
        {
            return DBEntities.DUOJU_IDENTIFIERS.SingleOrDefault(p => p.IDENTIFIER_NO == identifierNO);
        }
    }
}
