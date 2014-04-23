using DUOJU.Domain.Models.Party;

namespace DUOJU.WECHAT.Models.Party
{
    public class ViewPartyViewModel
    {
        public bool IsReturn { get; set; }

        public int? ParticipantId { get; set; }

        public PartyInfo PartyInfo { get; set; }
    }
}