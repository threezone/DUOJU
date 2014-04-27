using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.User;

namespace DUOJU.Dao.Abstract
{
    public interface IUserRepository
    {
        int SaveChanges();

        void AddUser(DUOJU_USERS user);

        DUOJU_ROLE_PRIVILEGES GetRolePrivilege(string role);

        DUOJU_USERS GetUserById(int id);

        DUOJU_USERS GetUserByOpenId(string openId);

        UserFinanceInfo GetUserFinanceInfoByOpenId(string openId);
    }
}
