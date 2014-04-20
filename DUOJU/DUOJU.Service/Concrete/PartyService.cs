using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Models.Party;
using DUOJU.Service.Abstract;
using System;

namespace DUOJU.Service.Concrete
{
    public class PartyService : IPartyService
    {
        private IPartyRepository PartyRepository { get; set; }

        private IUserRepository UserRepository { get; set; }


        public PartyService()
        {
            PartyRepository = new PartyRepository();
            UserRepository = new UserRepository();
        }


        public int AddParty(PublishPartyModel partyModel)
        {
            var user = UserRepository.GetUserByOpenId(partyModel.OpenId);
            var party = new DUOJU_PARTIES
            {
                //INITIATOR_ID = user.USER_ID,
                DUOJU_USERS = user,
                HOLD_DATE = partyModel.HoldDate.Value,
                HOLD_TIME = (int)partyModel.HoldTime.Value,
                DESCRIPTION = partyModel.Description,
                MIN_INTO_FORCE = partyModel.MinIntoForce.Value,
                MAX_INTO_FORCE = partyModel.MaxIntoForce,
                STATUS = (int)PartyStatuses.PUBLISHED,
                CREATE_BY = user.USER_ID,
                CREATE_TIME = DateTime.Now,
                LAST_UPDATE_BY = user.USER_ID,
                LAST_UPDATE_TIME = DateTime.Now
            };

            PartyRepository.AddParty(party);
            PartyRepository.SaveChanges();

            return party.PARTY_ID;
        }
    }
}
