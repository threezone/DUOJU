using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Abstract
{
    public interface IAreaRepository
    {
        DUOJU_COUNTRIES GetCountryInfoById(int countryId);

        DUOJU_COUNTRIES GetCountryInfoByName(string countryName);


        DUOJU_PROVINCES GetProvinceInfoById(int provinceId);

        DUOJU_PROVINCES GetProvinceInfoByName(string provinceName);


        DUOJU_CITIES GetCityInfoById(int cityId);

        DUOJU_CITIES GetCityInfoByName(string cityName);
    }
}
