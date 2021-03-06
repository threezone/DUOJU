﻿using System;
using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.FRAMEWORK.WeChat;
using DUOJU.Service.Abstract;
using DUOJU.Domain.Models.User;
using System.Collections.Generic;

namespace DUOJU.Service.Concrete
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; set; }

        private IAreaRepository AreaRepository { get; set; }


        public UserService()
        {
            UserRepository = new UserRepository();
            AreaRepository = new AreaRepository();
        }


        public int AddWeChatUser(WeChatUserInfo info)
        {
            var user = UserRepository.GetUserByOpenId(info.openid);
            var isAdd = user == null;

            var subscribed = info.subscribe.HasValue ?
                (YesNo)Enum.Parse(typeof(YesNo), info.subscribe.Value.ToString()) :
                (user != null && user.SUBSCRIBED == YesNo.Y.ToString() ? YesNo.Y : YesNo.N);

            if (isAdd)
            {
                var rolePrivilege = UserRepository.GetRolePrivilege(UserRoles.USER.ToString());
                user = new DUOJU_USERS
                {
                    ACCOUNT = string.Format(CommonSettings.USERACCOUNT_WECHAT_FORMAT, info.openid),
                    SOURCE = (int)UserSources.WECHAT,
                    DUOJU_ROLE_PRIVILEGES = rolePrivilege,
                    PRIVILEGES = rolePrivilege.PRIVILEGES,
                    OPEN_ID = info.openid,
                    CREATE_BY = CommonSettings.OPERATOR_SYSTEM_ID,
                    CREATE_TIME = DateTime.Now
                };
            }

            user.SUBSCRIBED = subscribed.ToString();
            if (subscribed == YesNo.Y)
            {
                if (info.subscribe_time.HasValue)
                    user.SUBSCRIBE_TIME = WeChat.ConvertDateTime(info.subscribe_time.Value);
                user.NICK_NAME = info.nickname;
                if (info.sex.HasValue)
                    user.SEX = ((UserSexes)Enum.Parse(typeof(UserSexes), info.sex.Value.ToString())).ToString();
                user.HEAD_IMG_URL = info.headimgurl;
                if (!string.IsNullOrEmpty(info.country))
                {
                    var country = AreaRepository.GetCountryInfoByName(info.country);
                    if (country != null)
                        user.DUOJU_COUNTRIES = country;
                }
                if (!string.IsNullOrEmpty(info.province))
                {
                    var province = AreaRepository.GetProvinceInfoByName(info.province);
                    if (province != null)
                        user.DUOJU_PROVINCES = province;
                }
                if (!string.IsNullOrEmpty(info.city))
                {
                    var city = AreaRepository.GetCityInfoByName(info.city);
                    if (city != null)
                        user.DUOJU_CITIES = city;
                }
            }
            user.ENABLED = YesNo.Y.ToString();
            user.LAST_UPDATE_BY = CommonSettings.OPERATOR_SYSTEM_ID;
            user.LAST_UPDATE_TIME = DateTime.Now;

            if (isAdd)
            {
                user.DUOJU_USER_FINANCES.Add(new DUOJU_USER_FINANCES
                {
                    COIN_COUNT = CommonSettings.USERREGISTER_DEFAULT_COIN_COUNT,
                    CREATE_BY = CommonSettings.OPERATOR_SYSTEM_ID,
                    CREATE_TIME = DateTime.Now,
                    LAST_UPDATE_BY = CommonSettings.OPERATOR_SYSTEM_ID,
                    LAST_UPDATE_TIME = DateTime.Now
                });

                UserRepository.AddUser(user);
            }
            UserRepository.SaveChanges();

            return user.USER_ID;
        }

        public int AddWeChatUser(WeChatUserInfo_OAuth info)
        {
            return AddWeChatUser(new WeChatUserInfo
            {
                openid = info.openid,
                nickname = info.nickname,
                sex = info.sex,
                city = info.city,
                province = info.province,
                country = info.country,
                headimgurl = info.headimgurl
            });
        }

        public void WeChatUserUnsubscribe(string openId)
        {
            var user = UserRepository.GetUserByOpenId(openId);
            if (user != null)
            {
                user.SUBSCRIBED = YesNo.N.ToString();
                user.LAST_UPDATE_BY = CommonSettings.OPERATOR_SYSTEM_ID;
                user.LAST_UPDATE_TIME = DateTime.Now;

                UserRepository.SaveChanges();
            }
        }

        public UserFinanceInfo GetUserFinanceInfoByOpenId(string openId)
        {
            return UserRepository.GetUserFinanceInfoByOpenId(openId);
        }
    }
}
