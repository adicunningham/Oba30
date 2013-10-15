﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Oba30
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                "Category",
                "Category/{category}",
                new {controller = "Blog", action = "Category"}
                );

            routes.MapRoute(
                "Tag",
                "Tag/{Tag}",
                new {controller = "Blog", action = "Tag"}
            );

            routes.MapRoute(
                "Post",
                "Archive/{year}/{month}/{title}",
                new {controller = "Blog", action = "Post"}
            );

            routes.MapRoute(
                "Login",
                "Login",
                new {controller = "Admin", action = "Login"}
            );

            routes.MapRoute(
                name: "Action",
                url: "{action}",
                defaults: new { controller = "Blog", action = "Posts"}
            );

        }
    }
}