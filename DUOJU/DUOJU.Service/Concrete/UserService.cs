using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Enums;
using DUOJU.Domain.Helpers;
using DUOJU.FRAMEWORK.WeChat;
using DUOJU.Service.Abstract;
using System;

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


        public int? AddWeChatUser(WeChatUserInfo info)
        {
            if (info.subscribe == (int)YesNo.Y)
            {
                var subscribed = (YesNo)Enum.Parse(typeof(YesNo), info.subscribe.ToString());

                var user = UserRepository.GetUserByOpenId(info.openid);
                var isAdd = false;
                if (user == null)
                {
                    isAdd = true;
                    var rolePrivilege = UserRepository.GetRolePrivilege(UserRoles.USER.ToString());

                    user = new DUOJU_USERS
                    {
                        ACCOUNT = string.Format(CommonSettings.USERACCOUNT_WECHAT_FORMAT, info.openid),
                        COME_FROM = (int)UserComeFroms.WECHAT,
                        //ROLE = UserRoles.USER.ToString(),
                        DUOJU_ROLE_PRIVILEGES = rolePrivilege,
                        PRIVILEGES = rolePrivilege.PRIVILEGES,
                        OPEN_ID = info.openid,
                        CREATE_BY = CommonSettings.OPERATOR_SYSTEM_ID,
                        CREATE_TIME = DateTime.Now
                    };
                }

                user.SUBSCRIBED = subscribed.ToString();
                user.SUBSCRIBE_TIME = DateTimeHelper.ConvertDateTime(info.subscribe_time.Value);
                if (subscribed == YesNo.Y)
                {
                    user.NICK_NAME = info.nickname;
                    user.SEX = ((UserSexes)Enum.Parse(typeof(UserSexes), info.sex.Value.ToString())).ToString();
                    user.HEAD_IMG_URL = info.headimgurl;
                    var country = AreaRepository.GetCountryInfoByName(info.country);
                    if (country != null)
                    {
                        //user.COUNTRY_ID = country.COUNTRY_ID;
                        user.DUOJU_COUNTRIES = country;
                    }
                    var province = AreaRepository.GetProvinceInfoByName(info.province);
                    if (province != null)
                    {
                        //user.PROVINCE_ID = province.PROVINCE_ID;
                        user.DUOJU_PROVINCES = province;
                    }
                    var city = AreaRepository.GetCityInfoByName(info.city);
                    if (city != null)
                    {
                        //user.CITY_ID = city.CITY_ID;
                        user.DUOJU_CITIES = city;
                    }
                }
                user.ENABLED = YesNo.Y.ToString();
                user.LAST_UPDATE_BY = CommonSettings.OPERATOR_SYSTEM_ID;
                user.LAST_UPDATE_TIME = DateTime.Now;

                if (isAdd)
                {
                    user.DUOJU_USER_CREDITS.Add(new DUOJU_USER_CREDITS
                    {
                        //USER_CREDIT_ID = user.USER_ID,
                        //DUOJU_USERS = user,
                        CREDIT_AMOUNT = CommonSettings.DEFAULT_USER_CREDIT_AMOUNT,
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

            return null;
        }
    }
}
