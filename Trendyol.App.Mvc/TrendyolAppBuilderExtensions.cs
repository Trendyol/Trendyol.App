using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Trendyol.App.Mvc
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseMvc(this TrendyolAppBuilder builder, Action<RouteCollection> registerRoutesAction = null)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RouteTable.Routes.LowercaseUrls = true;
            RouteTable.Routes.MapMvcAttributeRoutes();
            registerRoutesAction?.Invoke(RouteTable.Routes);

            return builder;
        }
    }
}
