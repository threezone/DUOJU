using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    public class ProspectiveSupplierController : Controller
    {
        //
        // GET: /ProspectiveSupplier/

        public ActionResult Index()
        {
            var response = HttpWebResponseUtility.CreateGetHttpResponse("http://www.meituan.com/deal/469993.html", null, string.Empty, null);
            var responseContent = HttpWebResponseUtility.GetResponseContent(response);
            return View();
        }

    }
}
