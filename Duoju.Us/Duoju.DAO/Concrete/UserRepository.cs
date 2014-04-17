using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;
using Duoju.DAO.Abstract;
using NHibernate.Criterion;
using Spring.Data.NHibernate.Generic;
using Spring.Data.NHibernate.Generic.Support;

namespace Duoju.DAO.Concrete
{
    public class UserRepository : HibernateDaoSupport, IUserRepository
    {
        public UserInfo GetUserById(int uid)
        {
            return HibernateTemplate.Get<UserInfo>(uid);
        }

        public UserInfo GetUserByAdminId(int usid)
        {
            var result = HibernateTemplate.ExecuteFind<UserInfo>(x => 
            {
                var c = x.CreateCriteria<UserInfo>()
                         .Add(Expression.Eq("AdminUserId", usid));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<UserInfo>();
            });
            return result.FirstOrDefault<UserInfo>();
        }

        public int AddUserInfo(UserInfo model)
        {
            return (int)HibernateTemplate.Save(model);
        }

        public IList<UserInfo> GetUserListByGroupId(int gid)
        {
            return HibernateTemplate.ExecuteFind<UserInfo>(x =>
                {
                    var c = x.CreateCriteria<UserInfo>()
                             .Add(Expression.Eq("GroupId", gid));
                    HibernateTemplate.PrepareCriteria(c);
                    return c.List<UserInfo>();
                });
        }

        public bool UpdateUser(UserInfo model)
        {
            try
            {
                HibernateTemplate.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
