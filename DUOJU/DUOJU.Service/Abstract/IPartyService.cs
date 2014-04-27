using System;
using System.Collections.Generic;
using DUOJU.Domain.Models.Party;

namespace DUOJU.Service.Abstract
{
    public interface IPartyService
    {
        int AddParty(PublishPartyInfo partyInfo);

        PartyInfo GetPartyInfo(int partyId);

        IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId);

        PartyParticipateCountInfo ParticipateParty(int partyId, string openId);

        ConfirmPartyInfo ConfirmParty(int partyId, string openId);

        IList<PartyInfo> GetPartyInfosByCreateUser(string openId);

        IList<PartyInfo> GetPartyInfosByParticipantUser(string openId);
    }
}
