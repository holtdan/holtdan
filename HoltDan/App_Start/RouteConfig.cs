using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HoltDan
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Pics",
                url: "Photos/{id}",
                defaults: new { controller = "Photos", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Scales",
                url: "Scales",
                defaults: new { controller = "Home", action = "Scales" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
