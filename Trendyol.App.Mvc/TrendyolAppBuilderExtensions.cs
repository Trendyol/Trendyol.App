using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace Trendyol.App.Mvc
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseMvc(this TrendyolAppBuilder builder, Action<RouteCollection> registerRoutesAction = null, Action<BundleCollection> registerBundles = null)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RouteTable.Routes.LowercaseUrls = true;
            RouteTable.Routes.MapMvcAttributeRoutes();
            registerRoutesAction?.Invoke(RouteTable.Routes);
            registerBundles?.Invoke(BundleTable.Bundles);

            return builder;
        }
    }
}
