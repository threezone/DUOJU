using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Abstract
{
    public interface IUserRepository
    {
        int SaveChanges();

        DUOJU_ROLE_PRIVILEGES GetRolePrivilege(string role);

        DUOJU_USERS GetUserByOpenId(string openId);

        void AddUser(DUOJU_USERS user);
    }
}
