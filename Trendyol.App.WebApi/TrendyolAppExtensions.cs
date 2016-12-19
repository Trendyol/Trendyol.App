using System.Web.Http;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppExtensions
    {
        public static HttpConfiguration GetHttpConfiguration(this TrendyolApp app)
        {
            HttpConfiguration config = app.GetData<HttpConfiguration>(Constants.TrendyolAppDataKeyPrefix + Constants.HttpConfigurationDataKey);
            return config;
        }
    }
}
