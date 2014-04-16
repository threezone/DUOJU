using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Web.Routing;
using YCF.CRM.Model.Exception;

namespace YCF.CRM.Helper.Sys.Attribute
{
    public class HandleExceptionAttribute: FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                return;

            var logger = LogManager.GetLogger(this.GetType());

            var exception = filterContext.Exception;

            if ((exception is YCFException) || (exception is System.InvalidOperationException))
            {
                var errorCode = 500;
                if (exception is System.InvalidOperationException) 
                {
                    errorCode = 404;
                }

                filterContext.HttpContext.Response.Clear();
                filterContext.ExceptionHandled = true;

                var httpException = new HttpException(null, exception);

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("httpException", httpException);
                routeData.Values.Add("errorCode", errorCode);

                ControllerBuilder.Current
                        .GetControllerFactory()
                        .CreateController(new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData), "Error")
                        .Execute(new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData));
            }
            else if (exception is ArgumentException && exception.Message.StartsWith("The parameters dictionary contains a null entry for parameter"))
            {
                return;
            }
            else
            {
                logger.Error(string.Format("controller: {0}, action: {1} 在执行时抛出异常, 请求参数: {2}", filterContext.Controller.GetType().FullName,
                        filterContext.RequestContext.RouteData.Values["action"], filterContext.HttpContext.Request.Params), filterContext.Exception);
            }
        }
    }
}