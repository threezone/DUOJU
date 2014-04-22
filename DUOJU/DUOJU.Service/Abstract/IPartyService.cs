using DUOJU.Domain.Models.Party;
using System.Collections.Generic;

namespace DUOJU.Service.Abstract
{
    public interface IPartyService
    {
        int AddParty(PublishPartyInfo partyInfo);

        PartyInfo GetPartyInfo(int partyId);

        IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId);
    }
}
