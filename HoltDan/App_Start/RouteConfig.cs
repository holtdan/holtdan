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

            //routes.MapRoute(
            //    name: "Pics",
            //    url: "Photos/{id}",
            //    defaults: new { controller = "Photos", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "Scales",
            //    url: "Scales",
            //    defaults: new { controller = "Home", action = "Scales" }
            //);
            //routes.MapRoute(
            //    name: "Frets",
            //    url: "Frets",
            //    defaults: new { controller = "Home", action = "Frets" }
            //);
            routes.MapRoute(
                name: "Play",
                url: "Play/{artist}/{album}",
                defaults: new { controller = "Home", action = "Play" }
            );
            routes.MapRoute(
                name: "Show",
                url: "Show/{id}/{album}",
                defaults: new { controller = "Home", action = "Show" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
