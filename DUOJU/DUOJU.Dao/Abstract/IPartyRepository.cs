using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Abstract
{
    public interface IPartyRepository
    {
        int SaveChanges();

        void AddParty(DUOJU_PARTIES party);
    }
}
