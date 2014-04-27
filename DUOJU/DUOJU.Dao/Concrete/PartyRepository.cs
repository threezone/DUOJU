using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.Party;
using System.Collections.Generic;
using System.Linq;

namespace DUOJU.Dao.Concrete
{
    public class PartyRepository : Repository, IPartyRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public void AddParty(DUOJU_PARTIES party)
        {
            DBEntities.DUOJU_PARTIES.Add(party);
        }

        public DUOJU_PARTIES GetPartyById(int partyId)
        {
            return DBEntities.DUOJU_PARTIES.SingleOrDefault(p => p.PARTY_ID == partyId);
        }

        public IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId)
        {
            return (from pp in DBEntities.DUOJU_PARTY_PARTICIPANTS
                    where pp.PARTY_ID == partyId
                    orderby pp.PARTICIPATE_TIME
                    select new PartyParticipantInfo
                    {
                        ParticipantId = pp.PARTICIPANT_ID,
                        ParticipantName = pp.DUOJU_USERS.NICK_NAME,
                        ParticipateTime = pp.PARTICIPATE_TIME,
                        Status = pp.STATUS
                    }).ToList();
        }

        public IList<PartyCommentInfo> GetPartyCommentInfos(int partyId)
        {
            return null;
        }

        public IList<PartyInfo> GetPartyInfosByCreateUser(string openId)
        {
            return (from u in DBEntities.DUOJU_USERS
                    join p in DBEntities.DUOJU_PARTIES on u.USER_ID equals p.INITIATOR_ID
                    where u.OPEN_ID == openId
                    select new PartyInfo
                    {
                        PartyId = p.PARTY_ID,
                        InitiatorName = u.NICK_NAME,
                        HoldDate = p.HOLD_DATE,
                        HoldTime = p.HOLD_TIME,
                        Description = p.DESCRIPTION,
                        MinIntoForce = p.MIN_INTO_FORCE,
                        MaxIntoForce = p.MAX_INTO_FORCE,
                        Status = p.STATUS
                    }).ToList();
        }

        public IList<PartyInfo> GetPartyInfosByParticipantUser(string openId)
        {
            return (from p in DBEntities.DUOJU_PARTIES
                    join pp in DBEntities.DUOJU_PARTY_PARTICIPANTS on p.PARTY_ID equals pp.PARTY_ID
                    join u in DBEntities.DUOJU_USERS on pp.PARTICIPANT_ID equals u.USER_ID
                    where u.OPEN_ID == openId
                    select new PartyInfo
                    {
                        PartyId = p.PARTY_ID,
                        InitiatorName = p.DUOJU_USERS.NICK_NAME,
                        HoldDate = p.HOLD_DATE,
                        HoldTime = p.HOLD_TIME,
                        Description = p.DESCRIPTION,
                        MinIntoForce = p.MIN_INTO_FORCE,
                        MaxIntoForce = p.MAX_INTO_FORCE,
                        Status = p.STATUS
                    }).ToList();
        }
    }
}
