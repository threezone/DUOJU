using System;
using System.Collections.Generic;
using System.Threading;
using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Exceptions;
using DUOJU.FRAMEWORK.WeChat;
using DUOJU.Service.Abstract;

namespace DUOJU.Service.Concrete
{
    public class IdentifierService : IIdentifierService
    {
        private IIdentifierRepository IdentifierRepository { get; set; }


        public IdentifierService()
        {
            IdentifierRepository = new IdentifierRepository();
        }


        public DUOJU_IDENTIFIERS GenerateIdentifier(IdentifierTypes type, DateTime expiresTime, IList<KeyValuePair<string, string>> settings, int operatorId)
        {
            DateTime createTime;
            string identifierNO;
            while (true)
            {
                createTime = DateTime.Now;
                identifierNO = string.Format(
                    "{0}{1}",
                    ((int)type).ToString().PadLeft(2, '0'),
                    WeChat.ConvertTimeStamp(createTime)
                );

                if (IdentifierRepository.IsIdentifierNOUnique(identifierNO))
                    break;

                Thread.Sleep(1);
            }

            var identifier = new DUOJU_IDENTIFIERS
            {
                IDENTIFIER_TYPE = (int)type,
                IDENTIFIER_NO = identifierNO,
                EXPIRES_TIME = expiresTime,
                STATUS = (int)IdentifierStatuses.NEW,
                CREATE_BY = operatorId,
                CREATE_TIME = createTime,
                LAST_UPDATE_BY = operatorId,
                LAST_UPDATE_TIME = DateTime.Now
            };

            if (settings != null && settings.Count > 0)
            {
                foreach (var setting in settings)
                {
                    identifier.DUOJU_IDENTIFIER_SETTINGS.Add(new DUOJU_IDENTIFIER_SETTINGS
                    {
                        SETTING_CODE = setting.Key,
                        SETTING_VALUE = setting.Value,
                        CREATE_BY = operatorId,
                        CREATE_TIME = DateTime.Now,
                        LAST_UPDATE_BY = operatorId,
                        LAST_UPDATE_TIME = DateTime.Now
                    });
                }
            }

            IdentifierRepository.AddIdentifier(identifier);
            IdentifierRepository.SaveChanges();

            return identifier;
        }

        public bool VerifyIdentifier(string identifierNO, int operatorId)
        {
            var identifier = IdentifierRepository.GetIdentifierByIdentifierNO(identifierNO);
            if (identifier == null)
                throw new CanNotFindIdentifierException();
            else
            {
                if (identifier.STATUS == (int)IdentifierStatuses.USED)
                    throw new IdentifierHasBeenUsedException();
                else if (identifier.EXPIRES_TIME > DateTime.Now)
                    throw new IdentifierHasBeenExpiredException();

                identifier.STATUS = (int)IdentifierStatuses.USED;
                identifier.LAST_UPDATE_BY = operatorId;
                identifier.LAST_UPDATE_TIME = DateTime.Now;

                IdentifierRepository.SaveChanges();
                return true;
            }
        }
    }
}
