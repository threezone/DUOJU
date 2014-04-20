﻿using System.Web.Mvc;
using System.Web.Routing;

namespace DUOJU.WECHAT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Party_PublishParty",
                url: "Party/PublishParty/{supplierId}/{openId}",
                defaults: new { controller = "Party", action = "PublishParty", supplierId = UrlParameter.Optional, openId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Party_ViewParty",
                url: "Party/ViewParty/{partyId}",
                defaults: new { controller = "Party", action = "ViewParty", partyId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "WeChat", action = "Index" }
            );
        }
    }
}