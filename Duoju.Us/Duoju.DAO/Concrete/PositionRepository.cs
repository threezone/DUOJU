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
    public class PositionRepository : HibernateDaoSupport, IPositionRepository
    {
        public Province GetProvinceByName(string name)
        {
            return HibernateTemplate.Execute<Province>(x => {
                var c = x.CreateCriteria<Province>()
                         .Add(Restrictions.Like("ProvinceName", "%" + name + "%"));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<Province>().FirstOrDefault();
            });
        }

        public IList<Province> GetAllProvince()
        {
            return HibernateTemplate.ExecuteFind<Province>(x =>{
                var c = x.CreateCriteria<Province>()
                         .AddOrder(Order.Asc("OrderSort"))
                         .AddOrder(Order.Asc("PinYinNameAbbr"));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<Province>();
            });
        }

        public City GetCityByName(string name)
        {
            return HibernateTemplate.Execute<City>(x =>
            {
                var c = x.CreateCriteria<City>()
                         .Add(Restrictions.Like("CityName", "%" + name + "%"));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<City>().FirstOrDefault();
            });
        }

        public IList<City> GetCityByProvinceCode(string code)
        {
            return HibernateTemplate.ExecuteFind<City>(x => {
                var c = x.CreateCriteria<City>()
                         .Add(Expression.Eq("ProvinceCode", code));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<City>();
            });
        }

        public District GetDistrictByName(string name)
        {
            return HibernateTemplate.Execute<District>(x =>
            {
                var c = x.CreateCriteria<District>()
                         .Add(Restrictions.Like("DistrictName", "%" + name + "%"));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<District>().FirstOrDefault();
            });
        }

        public IList<District> GetDistrictByCityCode(string code)
        {
            return HibernateTemplate.ExecuteFind<District>(x =>
            {
                var c = x.CreateCriteria<District>()
                         .Add(Expression.Eq("CityCode", code));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<District>();
            });
        }

        public City GetCityByCityCode(string code)
        {
            return HibernateTemplate.Execute<City>(x =>
            {
                var c = x.CreateCriteria<City>()
                         .Add(Expression.Eq("CityCode", code));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<City>().FirstOrDefault();
            });
        }

        public Province GetProvinceByProvinceCode(string code)
        {
            return HibernateTemplate.Execute<Province>(x =>
            {
                var c = x.CreateCriteria<Province>()
                         .Add(Expression.Eq("ProvinceCode", code));
                HibernateTemplate.PrepareCriteria(c);
                return c.List<Province>().FirstOrDefault();
            });
        }
    }
}
