using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MMRecordsUpdate
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles_css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles_js").Include(
                "~/scripts/jquery-1.10.2.js",
                "~/scripts/bootstrap.js",
                "~/scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/scripts/jquery.validate.extend.js"));
        }
    }
}