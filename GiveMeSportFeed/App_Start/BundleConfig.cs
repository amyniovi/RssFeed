using System.Web;
using System.Web.Optimization;

namespace GiveMeSportFeed
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                       "~/Scripts/angular.js",
                       "~/Scripts/angular-http-etag.js",
                        "~/App/app.js",
                        "~/App/rssFeedService.js",
                        "~/App/rssFeedController.js"
                ));
        }
    }
}
