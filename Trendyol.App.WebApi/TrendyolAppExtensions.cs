using System.Web.Http;
using Trendyol.App.WebApi.HealthCheck;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppExtensions
    {
        public static HttpConfiguration GetHttpConfiguration(this TrendyolApp app)
        {
            HttpConfiguration config = app.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);

            return config;
        }

        public static IHealthCheckerActivator GetHealthCheckerActivator(this TrendyolApp app)
        {
            IHealthCheckerActivator activator = app.DataStore.GetData<IHealthCheckerActivator>(Constants.HealthCheckerActivatorDataKey);

            if (activator == null)
            {
                activator = new DefaultHealthCheckerActivator();
                app.DataStore.SetData(Constants.HealthCheckerActivatorDataKey, activator);
            }

            return activator;
        }
    }
}