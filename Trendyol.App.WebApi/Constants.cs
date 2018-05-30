namespace Trendyol.App.WebApi
{
    public class Constants
    {
        public const string HttpConfigurationDataKey = "Trendyol.App.WebApi.HttpConfiguration";
        public const string HealthCheckerContainerDataKey = "Trendyol.App.WebApi.HealthCheckerContainerDataKey";
        public const string HealthCheckerActivatorDataKey = "Trendyol.App.WebApi.HealthCheckerActivatorDataKey";
        public const string ApiRootUrlDataKey = "Trendyol.App.WebApi.ApiRootUrlDataKey";
        public const string ExceptionHandlerDataKey = "Trendyol.App.WebApi.ExceptionHandlerDataKey";

        public struct MessageTypes
        {
            public const string NotFound = "not_found";
           
        }
    }
}
