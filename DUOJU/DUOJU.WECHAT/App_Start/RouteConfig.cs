using System.Web.Mvc;
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
                url: "Party/PublishParty/{supplierId}",
                defaults: new { controller = "Party", action = "PublishParty", supplierId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Party_ViewParty",
                url: "Party/ViewParty/{partyId}/{isReturn}/{participantId}",
                defaults: new { controller = "Party", action = "ViewParty", partyId = UrlParameter.Optional, isReturn = UrlParameter.Optional, participantId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Party_ParticipateParty",
                url: "Party/ParticipateParty/{partyId}",
                defaults: new { controller = "Party", action = "ParticipateParty", partyId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Party_ConfirmParty",
                url: "Party/ConfirmParty/{partyId}",
                defaults: new { controller = "Party", action = "ConfirmParty", partyId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "WeChat", action = "Index" }
            );
        }
    }
}