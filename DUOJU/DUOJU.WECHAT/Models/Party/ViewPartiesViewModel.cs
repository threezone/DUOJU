using System.Collections.Generic;
using DUOJU.Domain.Models.Party;

namespace DUOJU.WECHAT.Models.Party
{
    public class ViewPartiesViewModel
    {
        public IList<PartyInfo> PartyInfos { get; set; }
    }
}