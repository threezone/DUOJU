using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using C5;
using Common.Logging.Configuration;
using YCF.CRM.Models;
using YCF.CRM.Model.Admin.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    public class ResortController : BaseController
    {
        private IResortRepository SpResortRepository { get; set; }
        private IRoomRepository SpRoomRepository { get; set; }
        private IPositionRepository SpPositionRepository { get; set; }

        public ActionResult ProductManager(int sid)
        {
            var resort = SpResortRepository.GetResortByResortId(sid);
            if (resort.ResortInfo == null)
            {
                var resortInfo = new ResortInfo {ResortId = sid, CreatedDate = System.DateTime.Now};
                SpResortRepository.AddSupplierInfo(resortInfo);
                resort.ResortInfo = resortInfo;
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.PackageDetail))
            {
                resort.ResortInfo.PackageDetail = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/Package.html"), Encoding.UTF8);
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.BookNotice))
            {
                resort.ResortInfo.BookNotice = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/BookNotice.html"), Encoding.UTF8);
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.VendorInfo))
            {
                resort.ResortInfo.VendorInfo = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/VendorInfo.html"), Encoding.UTF8);
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.YcfComment))
            {
                resort.ResortInfo.YcfComment = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/YCFComment.html"), Encoding.UTF8);
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.TrafficGuide))
            {
                resort.ResortInfo.TrafficGuide = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/Traffic.html"), Encoding.UTF8);
            }
            if (string.IsNullOrEmpty(resort.ResortInfo.RecommendTrip))
            {
                resort.ResortInfo.RecommendTrip = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/Trip.html"), Encoding.UTF8);
            }
            return View(resort);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateResort(Resort model, string scenicIds, string themeIds)
        {
            var resort = SpResortRepository.GetResortByResortId(model.ResortId);
            resort.ResortName = model.ResortName;
            resort.ProviderPhone = model.ProviderPhone;
            resort.Telephone = model.Telephone;
            resort.Email = model.Email;
            resort.Fax = model.Fax;
            resort.Tags = model.Tags;
            resort.StartDate = model.StartDate;
            resort.EndDate = model.EndDate;
            resort.IsActive = model.IsActive;
            resort.ListInHolidayTheme = model.ListInHolidayTheme;
            resort.IsAllowCoupon = model.IsAllowCoupon;
            resort.SortOrder = model.SortOrder;
            resort.ReadCount = model.ReadCount;
            resort.SaledCount = model.SaledCount;
            /*历史遗留问题：经纬度是反的*/
            resort.Latitude = model.Longitude;
            resort.Longitude = model.Latitude;
            resort.GoogleCoordinate = model.Longitude + "," + model.Latitude;
            resort.Address = model.Address;
            resort.AreaId = model.AreaId;
            resort.ProvinceId = model.ProvinceId;
            resort.CityId = model.CityId;
            resort.DistrictId = model.DistrictId;
            resort.SaleStartDate = model.SaleStartDate;
            resort.SaleEndDate = model.SaleEndDate;
            SpResortRepository.UpdateResort(resort);

            resort.ResortInfo.RetailPrice = model.ResortInfo.RetailPrice;
            SpResortRepository.UpdateResortInfo(resort.ResortInfo);

            var scenicResortMappings = SpPositionRepository.GetScenicsByResortId(model.ResortId);
            //更新景区信息
            if (!string.IsNullOrEmpty(scenicIds))
            {
                var scenicIdList = scenicIds.TrimEnd(';').Split(';');
                if (scenicIdList.Length > 0)
                {
                    var delScenic = (from sm in scenicResortMappings
                        where !scenicIdList.Contains(sm.ScenicId.ToString())
                        select sm).ToArray();
                    var filterScenic = (from sm in scenicResortMappings
                        where scenicIdList.Contains(sm.ScenicId.ToString())
                        select sm).ToArray();
                    foreach (var s in delScenic)
                    {
                        SpPositionRepository.DeleteScenicMapping(model.ResortId, s.ScenicId);
                    }
                    foreach (var srm in scenicIdList.Where(si => !delScenic.Any(x => x.ScenicId.ToString().Equals(si)) &&
                                                                 !filterScenic.Any(y => y.ScenicId.ToString().Equals(si))).Select(si => new ScenicResortMappings {ResortId = model.ResortId, ScenicId = Convert.ToInt32(si)}))
                    {
                        SpPositionRepository.AddScenicMapping(srm);
                    }
                }
            }
            else
            {
                foreach (var s in scenicResortMappings)
                {
                    SpPositionRepository.DeleteScenicMapping(model.ResortId, s.ScenicId);
                }
            }

            var themeResortMappings = SpPositionRepository.GetThemesByResortId(model.ResortId);
            //更新主题信息
            if (!string.IsNullOrEmpty(themeIds))
            {
                var themeIdList = themeIds.TrimEnd(';').Split(';');
                if (themeIdList.Length > 0)
                {
                    var delTheme = (from sm in themeResortMappings
                                     where !themeIdList.Contains(sm.ThemeId.ToString())
                                     select sm).ToArray();
                    var filterTheme = (from sm in themeResortMappings
                                       where themeIdList.Contains(sm.ThemeId.ToString())
                                        select sm).ToArray();
                    foreach (var s in delTheme)
                    {
                        SpPositionRepository.DeleteThemeMapping(model.ResortId, s.ThemeId);
                    }
                    foreach (var trm in themeIdList.Where(si => !delTheme.Any(x => x.ThemeId.ToString().Equals(si)) &&
                                                                 !filterTheme.Any(y => y.ThemeId.ToString().Equals(si))).Select(si => new ThemeResortMappings() { ResortId = model.ResortId, ThemeId = Convert.ToInt32(si) }))
                    {
                        SpPositionRepository.AddThemeMapping(trm);
                    }
                }
            }
            else
            {
                foreach (var s in themeResortMappings)
                {
                    SpPositionRepository.DeleteThemeMapping(model.ResortId, s.ThemeId);
                }
            }
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateResortContent(int resortId, string content, string contentType)
        {
            var resortInfo = SpResortRepository.GetResortInfo(resortId);
            switch (contentType)
            {
                case "1":
                    resortInfo.PackageDetail = content;
                    break;
                case "2":
                    resortInfo.BookNotice = content;
                    break;
                case "3":
                    resortInfo.VendorInfo = content;
                    break;
                case "4":
                    resortInfo.YcfComment = content;
                    break;
                case "5":
                    resortInfo.RecommendTrip = content;
                    break;
                case "6":
                    resortInfo.TrafficGuide = content;
                    break;
            }
            SpResortRepository.UpdateResortInfo(resortInfo);
            var msg = new MessageModel { State = "Success", Message = "更新成功！" };
            return Content(JSONUtility.ToJson(msg, true));
        }

        [HttpPost]
        public ActionResult UpdateResortTitleInfo(int resortId, string mainTitle, string subTitle, string appMainTitle, string appSubTitle)
        {
            var resortInfo = SpResortRepository.GetResortInfo(resortId);
            resortInfo.MainTitle = mainTitle;
            resortInfo.Subtitle = subTitle;
            resortInfo.AppMainTitle = appMainTitle;
            resortInfo.AppSubTitle = appSubTitle;
            SpResortRepository.UpdateResortInfo(resortInfo);
            var msg = new MessageModel { State = "Success", Message = "更新成功！" };
            return Content(JSONUtility.ToJson(msg, true));
        }

        [HttpPost]
        public ActionResult GetRoomListByResortId(int rid)
        {
            var roomList = SpRoomRepository.GetRoomListByResortId(rid);
            var result = JSONUtility.ToJson(roomList, true);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetResortListBySearch(string keyWord, string rid, int page, int rows)
        {
            int total = 0;
            int resortId = 0;
            int.TryParse(rid, out resortId);
            var supplierList = SpResortRepository.SearchResorts(resortId, keyWord, (page - 1) * rows, rows, out total);
            var grid = new GridModel<System.Collections.Generic.IList<Resort>> {rows = supplierList, total = total};
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }
    }
}
