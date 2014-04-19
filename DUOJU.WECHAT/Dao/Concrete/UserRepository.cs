using Dao.Abstract;
using Domain.Entities;
using System.Linq;

namespace Dao.Concrete
{
    public class UserRepository : Repository, IUserRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public DUOJU_ROLE_PRIVILEGES GetRolePrivilege(string role)
        {
            return DBEntities.DUOJU_ROLE_PRIVILEGES.SingleOrDefault(rp => rp.ROLE == role);
        }

        public DUOJU_USERS GetUserByOpenId(string openId)
        {
            return DBEntities.DUOJU_USERS.SingleOrDefault(u => u.OPEN_ID == openId);
        }

        public void AddUser(DUOJU_USERS user)
        {
            DBEntities.DUOJU_USERS.Add(user);
        }
    }
}
