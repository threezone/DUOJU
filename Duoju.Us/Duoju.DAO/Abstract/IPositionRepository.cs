using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;

namespace Duoju.DAO.Abstract
{
    public interface IPositionRepository
    {
        Province GetProvinceByName(string name);
        IList<Province> GetAllProvince();
        City GetCityByName(string name);
        IList<City> GetCityByProvinceCode(string code);
        District GetDistrictByName(string name);
        IList<District> GetDistrictByCityCode(string code);
        City GetCityByCityCode(string code);
        Province GetProvinceByProvinceCode(string code);
    }
}
