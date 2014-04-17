using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Duoju.Model.Entity;
using Duoju.Models;
using Duoju.Model.Entity;
using Duoju.DAO.Abstract;
using Duoju.DAO.Utilities;

namespace Duoju.Controllers
{
    public class PositionController : BaseController
    {
        private IPositionRepository SpPositionRepository { get; set; }

        public ActionResult Index()
        {
            return View();
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
    }
}
