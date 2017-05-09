using System.Web.Http;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppExtensions
    {
        public static HttpConfiguration GetHttpConfiguration(this TrendyolApp app)
        {
            HttpConfiguration config = app.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);

            return config;
        }
    }
}