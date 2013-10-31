using System.Web;
using System.Web.Optimization;

namespace ManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // The Kendo JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/Kendo/kendo.web.min.js", // or kendo.all.min.js if you want to use Kendo UI Web and Kendo UI DataViz
                    "~/Scripts/Kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Styles/kendo").Include(
                   "~/Styles/kendo.common.min.css",
                   "~/Styles/kendo.black.min.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Styles/css").Include(
                      "~/Styles/bootstrap.css",
                      "~/Styles/site.css",
                      "~/Styles/bootstrap-responsive.css"));
        }
    }
}
