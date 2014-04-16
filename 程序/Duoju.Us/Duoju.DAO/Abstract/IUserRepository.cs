using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;

namespace Duoju.DAO.Abstract
{
    public interface IUserRepository
    {
        UserInfo GetUserById(int uid);
        UserInfo GetUserByAdminId(int usid);
        int AddUserInfo(UserInfo model);
        IList<UserInfo> GetUserListByGroupId(int gid);
        bool UpdateUser(UserInfo model);
    }
}
