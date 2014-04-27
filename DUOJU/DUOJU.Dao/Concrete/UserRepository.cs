using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.User;
using System.Linq;

namespace DUOJU.Dao.Concrete
{
    public class UserRepository : Repository, IUserRepository
    {
        public int SaveChanges()
        {
            return DBEntities.SaveChanges();
        }

        public void AddUser(DUOJU_USERS user)
        {
            DBEntities.DUOJU_USERS.Add(user);
        }

        public DUOJU_ROLE_PRIVILEGES GetRolePrivilege(string role)
        {
            return DBEntities.DUOJU_ROLE_PRIVILEGES.SingleOrDefault(rp => rp.ROLE == role);
        }

        public DUOJU_USERS GetUserById(int id)
        {
            return DBEntities.DUOJU_USERS.SingleOrDefault(u => u.USER_ID == id);
        }

        public DUOJU_USERS GetUserByOpenId(string openId)
        {
            return DBEntities.DUOJU_USERS.SingleOrDefault(u => u.OPEN_ID == openId);
        }

        public UserFinanceInfo GetUserFinanceInfoByOpenId(string openId)
        {
            return (from u in DBEntities.DUOJU_USERS
                    join uf in DBEntities.DUOJU_USER_FINANCES on u.USER_ID equals uf.USER_ID
                    where u.OPEN_ID == openId
                    select new UserFinanceInfo
                    {
                        CoinCount = uf.COIN_COUNT
                    }).SingleOrDefault();
        }
    }
}
