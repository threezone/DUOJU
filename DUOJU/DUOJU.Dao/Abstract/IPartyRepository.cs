using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.Party;
using System.Collections.Generic;

namespace DUOJU.Dao.Abstract
{
    public interface IPartyRepository
    {
        int SaveChanges();

        void AddParty(DUOJU_PARTIES party);

        DUOJU_PARTIES GetPartyById(int partyId);

        IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId);
    }
}
