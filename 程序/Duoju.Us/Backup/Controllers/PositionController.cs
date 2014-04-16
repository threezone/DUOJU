using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Model.Admin.Entity;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    public class PositionController : BaseController
    {
        private IPositionRepository SpPositionRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeAllAreas()
        {
            var areaList = SpPositionRepository.GeAllAreas();
            var jsonStr = JSONUtility.ToJson(areaList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetProvinceByAreaId(int areaId)
        {
            var provinceList = SpPositionRepository.GetaProvincesByAreaId(areaId);
            var jsonStr = JSONUtility.ToJson(provinceList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetAllProvince()
        {
            var provinceList = SpPositionRepository.GetAllProvince();
            var jsonStr = JSONUtility.ToJson(provinceList, true);
            return Content(jsonStr);
        }


        [HttpPost]
        public ActionResult GetCityByProvinceCode(string code)
        {
            var cityList = SpPositionRepository.GetCityByProvinceCode(code);
            var jsonStr = JSONUtility.ToJson(cityList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetDistrictByCityCode(string code)
        {
            var distictList = SpPositionRepository.GetDistrictByCityCode(code);
            var jsonStr = JSONUtility.ToJson(distictList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetScenicsByCityCode(string code)
        {
            var scenicList = SpPositionRepository.GetScenicsByCityCode(code);
            var jsonStr = JSONUtility.ToJson(scenicList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetThemeByCityCode(string code)
        {
            var themeList = SpPositionRepository.GetThemesByCityCode(code);
            var jsonStr = JSONUtility.ToJson(themeList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetScenicsMappingByResortId(int resortId)
        {
            var scenicResortMappings = SpPositionRepository.GetScenicsByResortId(resortId);
            var jsonStr = JSONUtility.ToJson(scenicResortMappings, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetThemesMappingByResortId(int resortId)
        {
            var themeResortMappings = SpPositionRepository.GetThemesByResortId(resortId);
            var jsonStr = JSONUtility.ToJson(themeResortMappings, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult AddOrDelScenicsMapping(int resortId, string scenicIds)
        {
            var scenicIdList = scenicIds.TrimEnd(';').Split(';');
            if (scenicIdList.Length > 0)
            {
                var scenicResortMappings = SpPositionRepository.GetScenicsByResortId(resortId);
                var delScenic = (from sm in scenicResortMappings
                                     where !scenicIdList.Contains(sm.ScenicId.ToString())
                                     select sm).ToArray();
                foreach (var s in delScenic)
                {
                    SpPositionRepository.DeleteScenicMapping(resortId, s.ScenicId);
                }
            }
            return Content("");
        }
    }
}
