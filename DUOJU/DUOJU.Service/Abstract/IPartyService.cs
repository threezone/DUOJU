﻿using System.Collections.Generic;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Models.Party;

namespace DUOJU.Service.Abstract
{
    public interface IPartyService
    {
        int AddParty(PublishPartyInfo partyInfo);

        PartyInfo GetPartyInfo(int partyId);

        IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId);

        PartyParticipateCountInfo ParticipateParty(int partyId, int userId);
    }
}
