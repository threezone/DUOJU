using DUOJU.Domain.Models.Party;

namespace DUOJU.WECHAT.Models.Party
{
    public class ViewPartyViewModel
    {
        public bool IsCreate { get; set; }

        public bool IsReturn { get; set; }

        public PartyInfo PartyInfo { get; set; }
    }
}