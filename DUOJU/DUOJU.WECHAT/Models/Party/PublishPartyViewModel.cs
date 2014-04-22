using DUOJU.Domain.Enums;
using DUOJU.Domain.Extensions;
using DUOJU.Domain.Models.Common;
using DUOJU.Domain.Models.Party;
using DUOJU.Domain.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DUOJU.WECHAT.Models.Party
{
    public class PublishPartyViewModel
    {
        public IList<KeyValue<PartyHoldTimes, string>> PartyHoldTimes
        {
            get
            {
                return Enum.GetValues(typeof(PartyHoldTimes))
                    .Cast<PartyHoldTimes>()
                    .Select(i => new KeyValue<PartyHoldTimes, string>(i, i.ToLocalize()))
                    .ToList();
            }
        }

        public SupplierInfo SupplierInfo { get; set; }

        public PublishPartyInfo PartyInfo { get; set; }
    }
}