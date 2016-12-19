using System.Web.Http;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppExtensions
    {
        public static HttpConfiguration GetHttpConfiguration(this TrendyolApp app)
        {
            HttpConfiguration config = app.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);
            return config;
        }
    }
}
