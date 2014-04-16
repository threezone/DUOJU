using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;
using System.Text;
using System.IO;

namespace YCF.CRM.Controllers
{
    public class OldCrmController : BaseController
    {
        private IOldCRM_AccountsRepository SpOldCrmAccountsRepository { get; set; }

        public ActionResult Index()
        {
            var oldSupplierList = SpOldCrmAccountsRepository.GetOldCrmSupplier();
            //Encoding.UTF8.GetString(Encoding.GetEncoding("latin1").GetBytes(dr[i].ToString()));
            foreach (var os in oldSupplierList)
            {
                var newStr = Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(os.Name));
            }
            return View();
        }

    }
}
