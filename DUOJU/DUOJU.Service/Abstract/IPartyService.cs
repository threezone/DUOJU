using DUOJU.Domain.Models.Party;

namespace DUOJU.Service.Abstract
{
    public interface IPartyService
    {
        int AddParty(PublishPartyModel partyModel);
    }
}
