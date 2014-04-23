using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.Party;
using System.Collections.Generic;
using System.Linq;

namespace DUOJU.Dao.Concrete
{
    public class PartyRepository : Repository, IPartyRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public void AddParty(DUOJU_PARTIES party)
        {
            DBEntities.DUOJU_PARTIES.Add(party);
        }

        public DUOJU_PARTIES GetPartyById(int partyId)
        {
            return DBEntities.DUOJU_PARTIES.SingleOrDefault(p => p.PARTY_ID == partyId);
        }

        public IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId)
        {
            return (from pp in DBEntities.DUOJU_PARTY_PARTICIPANTS
                    where pp.PARTY_ID == partyId
                    orderby pp.PARTICIPATE_TIME
                    select new PartyParticipantInfo
                    {
                        ParticipantId = pp.PARTICIPANT_ID,
                        ParticipantName = pp.DUOJU_USERS.NICK_NAME,
                        ParticipateTime = pp.PARTICIPATE_TIME,
                        Status = pp.STATUS
                    }).ToList();
        }
    }
}
