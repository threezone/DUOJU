using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;
using System.Linq;

namespace DUOJU.Dao.Concrete
{
    public class AreaRepository : Repository, IAreaRepository
    {
        public DUOJU_COUNTRIES GetCountryInfoById(int countryId)
        {
            return DBEntities.DUOJU_COUNTRIES.SingleOrDefault(c => c.COUNTRY_ID == countryId);
        }

        public DUOJU_COUNTRIES GetCountryInfoByName(string countryName)
        {
            return DBEntities.DUOJU_COUNTRIES.SingleOrDefault(c => c.COUNTRY_NAME.ToLower() == countryName.ToLower());
        }


        public DUOJU_PROVINCES GetProvinceInfoById(int provinceId)
        {
            return DBEntities.DUOJU_PROVINCES.SingleOrDefault(p => p.PROVINCE_ID == provinceId);
        }

        public DUOJU_PROVINCES GetProvinceInfoByName(string provinceName)
        {
            return DBEntities.DUOJU_PROVINCES.SingleOrDefault(p => p.PROVINCE_NAME.ToLower() == provinceName.ToLower());
        }


        public DUOJU_CITIES GetCityInfoById(int cityId)
        {
            return DBEntities.DUOJU_CITIES.SingleOrDefault(c => c.CITY_ID == cityId);
        }

        public DUOJU_CITIES GetCityInfoByName(string cityName)
        {
            return DBEntities.DUOJU_CITIES.SingleOrDefault(c => c.CITY_NAME.ToLower() == cityName.ToLower());
        }
    }
}
