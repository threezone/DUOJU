using DUOJU.Domain.Resources;
using DUOJU.WECHAT.App_Start;
using log4net;
using log4net.Config;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DUOJU.WECHAT
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog logger = LogManager.GetLogger(typeof(MvcApplication));


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Config/log4net.config.xml")));
            ModelMetadataProviders.Current = new CustomModelMetadataProvider(typeof(ModelResource), false);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception != null)
                logger.Error(exception);
        }
    }
}