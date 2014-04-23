using System.Linq;
using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Models.Party;
using DUOJU.Service.Abstract;
using System;
using System.Collections.Generic;
using DUOJU.Domain.Exceptions;

namespace DUOJU.Service.Concrete
{
    public class PartyService : IPartyService
    {
        private IPartyRepository PartyRepository { get; set; }

        private IUserRepository UserRepository { get; set; }

        private ISupplierService SupplierService { get; set; }


        public PartyService()
        {
            PartyRepository = new PartyRepository();
            UserRepository = new UserRepository();
            SupplierService = new SupplierService();
        }


        public int AddParty(PublishPartyInfo partyInfo)
        {
            var user = UserRepository.GetUserByOpenId(partyInfo.OpenId);
            var party = new DUOJU_PARTIES
            {
                SUPPLIER_ID = partyInfo.SupplierId.Value,
                DUOJU_USERS = user,
                HOLD_DATE = partyInfo.HoldDate.Value,
                HOLD_TIME = (int)partyInfo.HoldTime.Value,
                DESCRIPTION = partyInfo.Description,
                MIN_INTO_FORCE = partyInfo.MinIntoForce.Value,
                MAX_INTO_FORCE = partyInfo.MaxIntoForce,
                STATUS = (int)PartyStatuses.PUBLISHED,
                CREATE_BY = user.USER_ID,
                CREATE_TIME = DateTime.Now,
                LAST_UPDATE_BY = user.USER_ID,
                LAST_UPDATE_TIME = DateTime.Now
            };

            party.DUOJU_PARTY_PARTICIPANTS.Add(new DUOJU_PARTY_PARTICIPANTS
            {
                DUOJU_USERS = user,
                PARTICIPATE_TIME = DateTime.Now,
                STATUS = (int)PartyParticipantStatuses.PARTICIPATED,
                CREATE_BY = user.USER_ID,
                CREATE_TIME = DateTime.Now,
                LAST_UPDATE_BY = user.USER_ID,
                LAST_UPDATE_TIME = DateTime.Now
            });

            PartyRepository.AddParty(party);
            PartyRepository.SaveChanges();

            return party.PARTY_ID;
        }

        public PartyInfo GetPartyInfo(int partyId)
        {
            var party = PartyRepository.GetPartyById(partyId);
            if (party != null)
            {
                return new PartyInfo
                {
                    PartyId = party.PARTY_ID,
                    InitiatorOpenName = party.DUOJU_USERS.NICK_NAME,
                    HoldDate = party.HOLD_DATE,
                    HoldTime = (PartyHoldTimes)Enum.Parse(typeof(PartyHoldTimes), party.HOLD_TIME.ToString()),
                    Description = party.DESCRIPTION,
                    MinIntoForce = party.MIN_INTO_FORCE,
                    MaxIntoForce = party.MAX_INTO_FORCE,
                    Status = (PartyStatuses)Enum.Parse(typeof(PartyStatuses), party.STATUS.ToString()),
                    SupplierInfo = SupplierService.GetSupplierInfoById(party.SUPPLIER_ID),
                    PartyParticipantInfos = GetPartyParticipantInfos(partyId)
                };
            }

            return null;
        }

        public IList<PartyParticipantInfo> GetPartyParticipantInfos(int partyId)
        {
            var infos = PartyRepository.GetPartyParticipantInfos(partyId);
            infos.ToList()
                .ForEach(i =>
                {
                    i.EStatus = (PartyParticipantStatuses)Enum.Parse(typeof(PartyParticipantStatuses), i.Status.ToString());
                });

            return infos;
        }

        public PartyParticipateCountInfo ParticipateParty(int partyId, int userId)
        {
            var party = PartyRepository.GetPartyById(partyId);
            if (party == null)
                throw new CanNotFindPartyException();
            else
            {
                if (party.STATUS == (int)PartyStatuses.PUBLISHED)
                {
                    if (party.MAX_INTO_FORCE.HasValue && party.DUOJU_PARTY_PARTICIPANTS.Count + 1 >= party.MAX_INTO_FORCE.Value)
                    {
                        party.STATUS = (int)PartyStatuses.FULLED;
                        party.LAST_UPDATE_BY = userId;
                        party.LAST_UPDATE_TIME = DateTime.Now;
                    }

                    var user = UserRepository.GetUserById(userId);
                    party.DUOJU_PARTY_PARTICIPANTS.Add(new DUOJU_PARTY_PARTICIPANTS
                    {
                        DUOJU_USERS = user,
                        PARTICIPATE_TIME = DateTime.Now,
                        STATUS = (int)PartyParticipantStatuses.PARTICIPATED,
                        CREATE_BY = userId,
                        CREATE_TIME = DateTime.Now,
                        LAST_UPDATE_BY = userId,
                        LAST_UPDATE_TIME = DateTime.Now
                    });

                    PartyRepository.SaveChanges();
                    return new PartyParticipateCountInfo
                    {
                        MinIntoForce = party.MIN_INTO_FORCE,
                        MaxIntoForce = party.MAX_INTO_FORCE,
                        ParticipateCount = party.DUOJU_PARTY_PARTICIPANTS.Count
                    };
                }
                else if (party.STATUS == (int)PartyStatuses.DELETED)
                    throw new CanNotFindPartyException();
                else
                    throw new PartyWasClosedException();
            }
        }
    }
}
