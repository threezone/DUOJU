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
    public class ActionInfoRepository: HibernateDaoSupport, IActionInfoRepository
    {
        public IList<ActionInfo> GetActionInfoByGroupIdOrUserId(int? gid, int? uid)
        {
            var sql = string.Empty;

            if (!gid.HasValue && !uid.HasValue)
                return null;
            else if (gid.HasValue && !uid.HasValue)
            {
                sql = @"select aci.* from actionInfo aci,AuthorityInfo aui 
                        where aci.actionId=aui.actionId and aui.groupId=:groupId;";
                return HibernateTemplate.ExecuteFind<ActionInfo>(x => {
                    var q = x.CreateSQLQuery(sql);
                    HibernateTemplate.PrepareQuery(q);
                    return q.AddEntity(typeof(ActionInfo))
                            .SetInt32("groupId", gid.Value)
                            .List<ActionInfo>();
                });
            }
            else if (!gid.HasValue && uid.HasValue)
            {
                sql = @"select aci.* from actionInfo aci,AuthorityInfo aui 
                        where aci.actionId=aui.actionId and aui.userId=:userId;";
                return HibernateTemplate.ExecuteFind<ActionInfo>(x =>
                {
                    var q = x.CreateSQLQuery(sql);
                    HibernateTemplate.PrepareQuery(q);
                    return q.AddEntity(typeof(ActionInfo))
                            .SetInt32("userId", uid.Value)
                            .List<ActionInfo>();
                });
            }
            else
            {
                sql = @"select aci.* from actionInfo aci,AuthorityInfo aui 
                        where aci.actionId=aui.actionId 
                        and (aui.groupId=:groupId or aui.userId=:userId);";
                return HibernateTemplate.ExecuteFind<ActionInfo>(x =>
                {
                    var q = x.CreateSQLQuery(sql);
                    HibernateTemplate.PrepareQuery(q);
                    return q.AddEntity(typeof(ActionInfo))
                                .SetInt32("groupId", gid.Value)
                                .SetInt32("userId", uid.Value)
                                .List<ActionInfo>();
                });
            }
        }

        public IList<ActionInfo> GetAllAction()
        {
            return HibernateTemplate.LoadAll<ActionInfo>();
        }

        public IList<ActionInfo> GetActionByPath(string path)
        {
            return HibernateTemplate.ExecuteFind<ActionInfo>(x => 
            {
                var c = x.CreateCriteria<ActionInfo>()
                         .Add(Expression.Eq("UrlPath", path));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<ActionInfo>();
            });
        }
    }
}
