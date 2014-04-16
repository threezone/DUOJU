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
    public class AuthorityInfoRepository : HibernateDaoSupport, IAuthorityInfoRepository
    {
        public int AddAuthorityInfo(AuthorityInfo model)
        {
            try
            {
                return (int)HibernateTemplate.Save(model);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool RemoveAuthorityList(List<int> idList, int uid, int gid)
        {
            var authorityList = HibernateTemplate.ExecuteFind<AuthorityInfo>(x =>
            {
                var c = x.CreateCriteria<AuthorityInfo>()
                         .Add(Restrictions.And(Restrictions.In("ActionId", idList),
                              Restrictions.Or(Expression.Eq("UserId", uid), Expression.Eq("GroupId", gid))));
                return c.List<AuthorityInfo>();
            });
            foreach (var a in authorityList)
            {
                HibernateTemplate.Delete(a);
            }
            return true;
        }
    }
}
