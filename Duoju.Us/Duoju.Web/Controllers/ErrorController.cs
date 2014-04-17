using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Duoju.Models;
using Duoju.Model.Resource;
using Duoju.DAO.Utilities;
using Duoju.Model.Entity;

namespace Duoju.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            var model = new ErrorModel();

            var errorCode = "403";
            try
            {
                errorCode = RouteData.Values["errorCode"].ToString();
            }
            catch (NullReferenceException nex)
            {
                errorCode = "403";
            }
            int httpCode = 500;

            if (!string.IsNullOrEmpty(errorCode))
            {
                httpCode = Convert.ToInt32(errorCode);
            }
            else
            {
                var httpException = RouteData.Values["httpException"] as HttpException;
                httpCode = (httpException == null) ? 500 : httpException.GetHttpCode();
            }
            var dict = ResourceUtility.ErrorMessage(httpCode);
            model.ErrorCode = httpCode.ToString();
            model.Title = dict[string.Format("Error_{0}_Title", httpCode)];
            model.Message = dict[string.Format("Error_{0}_Message", httpCode)];

            return View("Error", model);
        }

    }
}
