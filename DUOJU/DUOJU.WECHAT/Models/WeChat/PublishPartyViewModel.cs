using DUOJU.Domain.Models.Party;
using DUOJU.Domain.Models.Supplier;

namespace DUOJU.WECHAT.Models.WeChat
{
    public class PublishPartyViewModel
    {
        public SupplierInfo SupplierInfo { get; set; }

        public PublishPartyModel PartyModel { get; set; }
    }
}