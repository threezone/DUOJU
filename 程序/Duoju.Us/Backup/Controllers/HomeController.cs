using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IActionInfoRepository SpActionInfoRepository { get; set; }

        public ActionResult Index()
        {
            return View(string.Empty);
        }

        [HttpPost]
        public ActionResult GetMenu()
        {
            IList<ActionInfo> actionInfoList = SpActionInfoRepository.GetActionInfoByGroupIdOrUserId(base.CurrentUser.GroupId, base.CurrentUserId);
            IList<MenuModel> menuList = MenuHandler(actionInfoList);
            var jsonStr = JSONUtility.ToJson(menuList, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetActionByGidAndUid(int gid, int uid)
        {
            IList<ActionInfo> actionInfoList = SpActionInfoRepository.GetActionInfoByGroupIdOrUserId(base.CurrentUser.GroupId, null);
            var tree = from a in actionInfoList
                       select new TreeModel { id = a.ActionId, pId = a.ParentId, name = a.ActionName, isGroupAuthority = true };
            IList<ActionInfo> actionInfoList2 = SpActionInfoRepository.GetActionInfoByGroupIdOrUserId(null, uid);
            var tree2 = from a in actionInfoList2
                       select new TreeModel { id = a.ActionId, pId = a.ParentId, name = a.ActionName, isGroupAuthority = false };
            var jsonStr = JSONUtility.ToJson(tree.Union(tree2), true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetAllAction()
        {
            IList<ActionInfo> actionInfoList = SpActionInfoRepository.GetAllAction();
            var tree = from a in actionInfoList
                       select new TreeModel { id = a.ActionId, pId = a.ParentId, name = a.ActionName, isParent = a.ParentId==0 };
            var jsonStr = JSONUtility.ToJson(tree, true);
            return Content(jsonStr);
        }

        protected IList<MenuModel> MenuHandler(IList<ActionInfo> actionInfoList)
        {
            var parentMenu = from p in actionInfoList
                             where p.ParentId == 0
                             select new MenuModel
                             {
                                 ActionId = p.ActionId,
                                 ActionName = p.ActionName,
                                 ParentId = p.ParentId,
                                 UrlPath = p.UrlPath,
                                 Icon = p.Icon,
                                 SubMenu = (from sp in actionInfoList
                                            where sp.ParentId == p.ActionId 
                                            select new MenuModel {
                                                ActionId = sp.ActionId,
                                                ActionName = sp.ActionName,
                                                ParentId = sp.ParentId,
                                                UrlPath = sp.UrlPath,
                                                Icon = sp.Icon,
                                                SubMenu = (from ssp in actionInfoList
                                                           where ssp.ParentId == sp.ActionId
                                                           select new MenuModel
                                                           {
                                                               ActionId = ssp.ActionId,
                                                               ActionName = ssp.ActionName,
                                                               ParentId = ssp.ParentId,
                                                               UrlPath = ssp.UrlPath,
                                                               Icon = ssp.Icon
                                                           }).ToList()
                                            }).ToList()
                             };
            return parentMenu.ToList();
        }
    }
}
