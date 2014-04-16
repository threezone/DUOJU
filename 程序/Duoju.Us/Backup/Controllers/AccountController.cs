using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthorityManager()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserInfo()
        {
            /*
            var resortList = SpResortsRepository.GetAllResorts();
            foreach (var resort in resortList)
            {
                var resortName = Regex.Replace(resort.ResortName, @"[（|\(|\[\{][\s\S]*[）|\)|\]|\}]", "");
                resortName = Regex.Replace(resortName, @"[＋|－|\+|\-|_][\s\S]*", "").Replace("套餐", "").Replace("双人", "").Replace("三人", "").Replace("日游", "");
                var supplierList = SpSupplierInfoRepository.GetSupplierByName(resortName);
                if (supplierList.Count() > 0)
                {
                    Province province = null;
                    City city = null;
                    District district = null;

                    if (!string.IsNullOrEmpty(resort.Province))
                    {
                        province = SpPositionRepository.GetProvinceByName(resort.Province);
                    }
                    if (!string.IsNullOrEmpty(resort.City))
                    {
                        city = SpPositionRepository.GetCityByName(resort.City);
                        district = SpPositionRepository.GetDistrictByName(resort.City);
                        if (city == null && district != null)
                        {
                            city = SpPositionRepository.GetCityByCityCode(district.CityCode);
                        }
                        if (city != null && province == null)
                        {
                            province = SpPositionRepository.GetProvinceByProvinceCode(city.ProvinceCode);
                        }
                    }
                    if (province != null || city != null || district != null)
                    {
                        foreach (var supplier in supplierList)
                        {
                            if (province != null)
                            {
                                supplier.ProvinceId = province.ProvinceId;
                            }
                            if (city != null)
                            {
                                supplier.CityId = city.CityId;
                            }
                            if (district != null)
                            {
                                supplier.DistrictId = district.DistrictId;
                            }
                            SpSupplierInfoRepository.UpdateSupplierInfo(supplier);
                        }
                    }
                }
            }*/
            HubbleUtility hubbleUtility = HubbleFactory.GetHubbleInstance();
            int total = 0;
            var supplierList = hubbleUtility.MatchForList<SupplierInfo>(new List<string> { "supplierName","address" }, 
                "美林湖", new Dictionary<string,object>(), out total);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetUserListByGroupId()
        {
            var userList = SpUserRepository.GetUserListByGroupId(CurrentUser.GroupId);
            var idList = (from uid in userList
                         select uid.AdminUserId).ToList();
            var adminUserList = SpUserAdminRepository.GetAdminUserByIdList(idList);
            foreach (var au in adminUserList)
            {
                au.Password = "";
            }
            var userModel = from u in userList
                            from a in adminUserList
                            where u.AdminUserId == a.Usid
                            select new UserInfoModel { CrmUserInfo = u, AdminUserInfo = a };
            return Content(JSONUtility.ToJson(userModel, true));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSimpleUserInfoListByGroupId()
        {
            var userList = SpUserRepository.GetUserListByGroupId(CurrentUser.GroupId);
            return Content(JSONUtility.ToJson(userList, true));
        }

        //
        // GET: /Account/
        [HttpPost]
        public ActionResult Login(string loginName, string password)
        {
            var user = SpUserAdminRepository.GetAdminUser(loginName);
            if (user != null && user.Password.ToLower().Equals(EncryptUtility.Md5(password).ToLower()))
            {
                var userInfo = base.SpUserRepository.GetUserByAdminId(user.Usid);
                if (userInfo == null&&user.Usid!=0)
                {
                    var userModel = new UserInfo
                    {
                        GroupId = 2,
                        Email = user.Email,
                        AdminUserId = user.Usid,
                        Name = user.UserName
                    };
                    var crmId = base.SpUserRepository.AddUserInfo(userModel);
                    userInfo = base.SpUserRepository.GetUserById(crmId);
                }
                System.Web.Security.FormsAuthentication.SetAuthCookie(userInfo.CRMUserId.ToString(), true);
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }
        
        [HttpPost]
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            var jsonResult =JSONUtility.ToJson(new MessageModel{
                                State = "success",
                                Message = "Logout Success"
                            }, true);
            return Content(jsonResult);
        }

        [HttpPost]
        public ActionResult GetCurrentUser()
        {
            var userInfo = SpUserAdminRepository.GetAdminUser(CurrentUser.AdminUserId);
            var jsonResult = JSONUtility.ToJson(userInfo, true);
            return Content(jsonResult);
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            var html = "var _input_userId = document.createElement('input');"
                         + "_input_userId.setAttribute('id', '_crm_userId');"
                         + "_input_userId.setAttribute('value', '{0}');"
                         + "document.getElementsByTagName('body')[0].appendChild(_input_userId);";
            try
            {
                return Content(string.Format(html, CurrentUserId));
            }
            catch (Exception ex)
            {
                return Content(string.Format(html, -1));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AuthorityManager(string addAuthority, string removeAuthority, int userId)
        {
            if (!string.IsNullOrEmpty(addAuthority))
            {
                var addAuthorityArray = addAuthority.Trim(',').Split(',').Select(s => int.Parse(s)).ToList();
                if (addAuthorityArray.Count() > 0)
                {
                    foreach (var aid in addAuthorityArray)
                    {
                        var authority = new AuthorityInfo();
                        authority.ActionId = aid;
                        authority.UserId = userId;
                        authority.CreateBy = base.CurrentUserId;
                        authority.AuthorityName = System.DateTime.Now.ToShortDateString() + "创建";
                        authority.CreateDate = System.DateTime.Now;
                        authority.Sort = 0;
                        SpAuthorityInfoRepository.AddAuthorityInfo(authority);
                    }
                }
            }
            if (!string.IsNullOrEmpty(removeAuthority))
            {
                var removeAuthorityArray = removeAuthority.Trim(',').Split(',').Select(s => int.Parse(s)).ToList();
                if (removeAuthorityArray.Count() > 0)
                {
                    SpAuthorityInfoRepository.RemoveAuthorityList(removeAuthorityArray, userId, base.CurrentUser.GroupId);
                }
            }
            return Content("");
        }
    }
}
