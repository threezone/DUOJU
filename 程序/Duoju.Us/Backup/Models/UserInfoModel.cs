using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YCF.CRM.Model.Entity;

namespace YCF.CRM.Models
{
    public class UserInfoModel
    {
        public UserInfo CrmUserInfo { get; set; }
        public User_Admin AdminUserInfo { get; set; }
    }
}