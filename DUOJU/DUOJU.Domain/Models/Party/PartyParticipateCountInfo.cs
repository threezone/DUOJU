namespace DUOJU.Domain.Models.Party
{
    public class PartyParticipateCountInfo
    {
        public string InitiatorOpenId { get; set; }

        public int MinIntoForce { get; set; }

        public int? MaxIntoForce { get; set; }

        public int ParticipateCount { get; set; }
    }
}
