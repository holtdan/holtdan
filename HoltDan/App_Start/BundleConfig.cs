using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web;
using System.Web.Optimization;

namespace HoltDan
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

            bundles.UseCdn = true;
            var cssTransformer = new StyleTransformer();
            var jsTransformer = new ScriptTransformer();
            var nullOrderer = new NullOrderer();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            var danJsBundle = new ScriptBundle("~/bundles/danJs");
            danJsBundle.Include(
                "~/Scripts/DanAudioMgr.js",
                "~/Scripts/DanScales.js"
                );
            danJsBundle.Transforms.Add(jsTransformer);
            danJsBundle.Orderer = nullOrderer;
            bundles.Add(danJsBundle);

            var cssBundle = new StyleBundle("~/bundles/css");
            cssBundle.Include("~/Content/css",
                      "~/Content/bootstrap.css",
                      "~/Content/HoltDan.less");
            cssBundle.Builder = new NullBuilder();
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);

            cssBundle = new StyleBundle("~/bundles/cssNoBS");
            cssBundle.Include("~/Content/css",
                      "~/Content/HoltDan.less",
                      "~/Content/DanScales.less");
            cssBundle.Builder = new NullBuilder();
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);
        }
    }
}
