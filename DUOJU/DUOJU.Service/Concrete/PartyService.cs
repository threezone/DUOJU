using System;
using System.Collections.Generic;
using System.Linq;
using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Exceptions;
using DUOJU.Domain.Models.Party;
using DUOJU.Service.Abstract;

namespace DUOJU.Service.Concrete
{
    public class PartyService : IPartyService
    {
        private IPartyRepository PartyRepository { get; set; }

        private IUserRepository UserRepository { get; set; }

        private ISupplierService SupplierService { get; set; }

        private IIdentifierService IdentifierService { get; set; }


        public PartyService()
        {
            PartyRepository = new PartyRepository();
            UserRepository = new UserRepository();
            SupplierService = new SupplierService();
            IdentifierService = new IdentifierService();
        }


        private void ConvertPartyInfo(PartyInfo info)
        {
            info.EHoldTime = (PartyHoldTimes)Enum.Parse(typeof(PartyHoldTimes), info.HoldTime.ToString());
            info.EStatus = (PartyStatuses)Enum.Parse(typeof(PartyStatuses), info.Status.ToString());
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
                var info = new PartyInfo
                {
                    PartyId = party.PARTY_ID,
                    InitiatorName = party.DUOJU_USERS.NICK_NAME,
                    HoldDate = party.HOLD_DATE,
                    HoldTime = party.HOLD_TIME,
                    Description = party.DESCRIPTION,
                    MinIntoForce = party.MIN_INTO_FORCE,
                    MaxIntoForce = party.MAX_INTO_FORCE,
                    Status = party.STATUS,
                    SupplierInfo = SupplierService.GetSupplierInfoById(party.SUPPLIER_ID),
                    PartyParticipantInfos = GetPartyParticipantInfos(partyId)
                };

                ConvertPartyInfo(info);
                return info;
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
                    var validParticipants = party.DUOJU_PARTY_PARTICIPANTS.Where(i => i.PARTY_ID == partyId && i.STATUS == (int)PartyParticipantStatuses.PARTICIPATED);
                    var participant = validParticipants.FirstOrDefault(i => i.PARTICIPANT_ID == userId);

                    if (participant != null && participant.STATUS == (int)PartyParticipantStatuses.PARTICIPATED)
                        throw new UserHasBeenParticipateThePartyException();

                    if (participant == null)
                    {
                        var user = UserRepository.GetUserById(userId);
                        participant = new DUOJU_PARTY_PARTICIPANTS
                        {
                            DUOJU_USERS = user,
                            CREATE_BY = userId,
                            CREATE_TIME = DateTime.Now
                        };

                        party.DUOJU_PARTY_PARTICIPANTS.Add(participant);
                        if (party.MAX_INTO_FORCE.HasValue && validParticipants.Count() + 1 >= party.MAX_INTO_FORCE.Value)
                        {
                            party.STATUS = (int)PartyStatuses.FULLED;
                            party.LAST_UPDATE_BY = userId;
                            party.LAST_UPDATE_TIME = DateTime.Now;
                        }
                    }

                    participant.PARTICIPATE_TIME = DateTime.Now;
                    participant.STATUS = (int)PartyParticipantStatuses.PARTICIPATED;
                    participant.LAST_UPDATE_BY = userId;
                    participant.LAST_UPDATE_TIME = DateTime.Now;

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

        public Tuple<string, DateTime> ConfirmParty(int partyId, string openId)
        {
            var party = PartyRepository.GetPartyById(partyId);
            if (party == null)
                throw new CanNotFindPartyException();
            else
            {
                if (party.DUOJU_USERS.OPEN_ID != openId)
                    throw new NoPartySInitiatorException();
                else if (party.STATUS == (int)PartyStatuses.CONFIRMED)
                    return new Tuple<string, DateTime>(party.DUOJU_IDENTIFIERS.IDENTIFIER_NO, party.DUOJU_IDENTIFIERS.EXPIRES_TIME);

                var expiresTime = DateTime.Now.AddDays(CommonSettings.IDENTIFIEREXPIRESTIME_DAY_PARTY);
                expiresTime = new DateTime(expiresTime.Year, expiresTime.Month, expiresTime.Day).AddDays(1).AddSeconds(-1);
                var identifier = IdentifierService.GenerateIdentifier(IdentifierTypes.PARTY, expiresTime, new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>(CommonSettings.IDENTIFIERSETTINGCODE_PARTYID, partyId.ToString())
                }, party.CREATE_BY);

                party.DUOJU_IDENTIFIERS = identifier;
                party.STATUS = (int)PartyStatuses.CONFIRMED;
                party.LAST_UPDATE_BY = party.CREATE_BY;
                party.LAST_UPDATE_TIME = DateTime.Now;

                PartyRepository.SaveChanges();

                return new Tuple<string, DateTime>(identifier.IDENTIFIER_NO, expiresTime);
            }
        }

        public IList<PartyInfo> GetPartyInfosByCreateUser(string openId)
        {
            var infos = PartyRepository.GetPartyInfosByCreateUser(openId);
            infos.ToList().ForEach(i => ConvertPartyInfo(i));

            return infos;
        }

        public IList<PartyInfo> GetPartyInfosByParticipantUser(string openId)
        {
            var infos = PartyRepository.GetPartyInfosByParticipantUser(openId);
            infos.ToList().ForEach(i => ConvertPartyInfo(i));

            return infos;
        }
    }
}
