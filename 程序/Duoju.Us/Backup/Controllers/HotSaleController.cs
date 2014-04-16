using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;
using System.Web.Security;

namespace YCF.CRM.Controllers
{
    [Authorize]
    public class HotSaleController : BaseController
    {
        private IHotSaleRepository SpHotSaleRepository { get; set; }
        private IPositionRepository SpPositionRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HotSaleDetial(int hid)
        {
            var hotSale = SpHotSaleRepository.GetHotSaleByHotSaleId(hid);
            return View(hotSale);
        }

        [HttpPost]
        public ActionResult SearchHotSaleByProductName(string keyWord, int page, int rows)
        {
            int total = 0;
            var hotSaleList = SpHotSaleRepository.SearchHotSaleByProductName(keyWord, (page - 1) * rows, rows, out total);
            GridModel<IList<HotSale>> grid = new GridModel<IList<HotSale>>();
            grid.rows = hotSaleList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetHotSaleList(int page, int rows)
        {
            int total = 0;
            var hotSaleList = SpHotSaleRepository.GetHotSaleList((page - 1) * rows, rows, out total);
            GridModel<IList<HotSale>> grid = new GridModel<IList<HotSale>>();
            grid.rows = hotSaleList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }
    }
}
