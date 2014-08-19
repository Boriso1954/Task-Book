using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TaskBook.WebApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/vendor/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                       "~/Scripts/vendor/angular.js",
                       "~/Scripts/vendor/angular-ng-grid.js",
                       "~/Scripts/vendor/angular-resource.js",
                       "~/Scripts/vendor/angular-route.js",
                       "~/Scripts/vendor/angular-local-storage.js",
                       "~/Scripts/vendor/loading-bar.js",
                       "~/Scripts/vendor/ui-bootstrap-tpls-0.11.0.js",
                       "~/Scripts/vendor/angular-filter.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/vendor/bootstrap.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                       "~/Scripts/app/app.js",
                       "~/Scripts/app/controllers/*.js",
                       "~/Scripts/app/services/*.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/loading-bar.css",
                      "~/Content/site.css",
                      "~/Content/app.css"
                      ));
        }
    }
}