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
    public class SupplierRepository : HibernateDaoSupport, ISupplierRepository
    {
        public IList<SupplierInfo> GetAllSupplierList(int pageStart, int pageLimit, out int total)
        {
            var _total = 0;
            HibernateTemplate.CacheQueries = false;
            var supplierList = HibernateTemplate.ExecuteFind<SupplierInfo>(x =>
            {
                var c = x.CreateCriteria<SupplierInfo>();
                _total = c.List().Count;
                c.SetFirstResult(pageStart)
                 .SetMaxResults(pageLimit);
                HibernateTemplate.PrepareCriteria(c);
                return c.List<SupplierInfo>();
            });
            total = _total;
            return supplierList;
        }

        public SupplierInfo GetSupplierBySupplierId(int sid)
        {
            return HibernateTemplate.Get<SupplierInfo>(sid);
        }

        public bool SaveOrUpdateSupplierInfo(SupplierInfo supplierInfo)
        {
            try
            {
                HibernateTemplate.SaveOrUpdate(supplierInfo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int AddSupplierInfo(SupplierInfo supplierInfo)
        {
            HibernateTemplate.CacheQueries = false;
            return (int)HibernateTemplate.Save(supplierInfo);
        }

        public bool UpdateSupplierInfo(SupplierInfo supplierInfo)
        {
            try
            {
                HibernateTemplate.CacheQueries = false;
                HibernateTemplate.Update(supplierInfo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteSupplierById(int sid)
        {
            var sql = "Delete from SupplierInfo Where SupplierId = :sid";
            return HibernateTemplate.Execute<bool>(x =>
            {
                var q = x.CreateQuery(sql)
                         .SetInt32("sid", sid)
                         .ExecuteUpdate();
                return q > 0;
            });
        }

        public bool DeleteSupplierByIdList(string sidList)
        {
            var sql = string.Format("Delete from SupplierInfo Where SupplierId in ({0})", sidList);
            return HibernateTemplate.Execute<bool>(x =>
            {
                var q = x.CreateQuery(sql)
                         .ExecuteUpdate();
                return q > 0;
            });
        }

        public IList<SupplierInfo> SearchSupplier(string keyWord, int pageStart, int pageLimit, out int total)
        {
            var _total = 0;
            var supplierList = HibernateTemplate.ExecuteFind<SupplierInfo>(x =>
            {
                var c = x.CreateCriteria<SupplierInfo>()
                         .Add(Restrictions.Like("Name", "%" + keyWord + "%"));
                _total = c.List().Count;
                c.SetFirstResult(pageStart)
                 .SetMaxResults(pageLimit);
                HibernateTemplate.PrepareCriteria(c);
                return c.List<SupplierInfo>();
            });
            total = _total;
            return supplierList;
        }

        public IList<SupplierInfo> GetSupplierByName(string name)
        {
            return HibernateTemplate.ExecuteFind<SupplierInfo>(x =>
            {
                var c = x.CreateCriteria<SupplierInfo>()
                         .Add(Restrictions.Like("SupplierName", "%" + name + "%"));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<SupplierInfo>();
            });
        }
    }
}
