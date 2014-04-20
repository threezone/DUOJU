using DUOJU.Dao.Abstract;

namespace DUOJU.Dao.Concrete
{
    public class PartyRepository : Repository, IPartyRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public void AddParty(Domain.Entities.DUOJU_PARTIES party)
        {
            DBEntities.DUOJU_PARTIES.Add(party);
        }
    }
}
