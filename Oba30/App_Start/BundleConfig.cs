using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Oba30.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
           // Use the CDN file for bundles if specified.
            bundles.UseCdn = true;
 
            // jquery library bundle
            var jqueryBundle = new ScriptBundle("~/jquery", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js")
                                    .Include("~/Scripts/jquery-{version}.js");
            bundles.Add(jqueryBundle);
 
            // jquery validation library bundle
            var jqueryValBundle = new ScriptBundle("~/jqueryval", "http://ajax.aspnetcdn.com/ajax/jquery.validate/1.10.0/jquery.validate.min.js")
                                        .Include("~/Scripts/jquery.validate.js");
            bundles.Add(jqueryValBundle );
 
            // jquery unobtrusive validation library
            var jqueryUnobtrusiveValBundle = new ScriptBundle("~/jqueryunobtrusiveval", "http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js")
                                                .Include("~/Scripts/jquery.validate.unobtrusive.js");
            bundles.Add(jqueryUnobtrusiveValBundle);
 
            // application script bundle
            var layoutJsBundle = new ScriptBundle("~/js").Include("~/Scripts/app.js");
            bundles.Add(layoutJsBundle );
 
            // css bundle
            var layoutCssBundle = new StyleBundle("~/css").Include("~/Content/themes/simple/style.css");
            bundles.Add(layoutCssBundle );
 
            // TODO: bundles for other pages        
        }
    }
}