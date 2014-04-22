using DUOJU.Domain.Enums;
using System;

namespace DUOJU.Domain.Models.Party
{
    public class PartyParticipantInfo
    {
        public string ParticipantName { get; set; }

        public DateTime ParticipateTime { get; set; }

        public int Status { get; set; }

        public PartyParticipantStatuses EStatus { get; set; }
    }
}
