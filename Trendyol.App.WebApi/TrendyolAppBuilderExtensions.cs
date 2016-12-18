using System.Configuration;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Trendyol.App.WebApi.Handlers;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppBuilderExtensions
    {
        private const string TrendyolAppDataKeyPrefix = "Trendyol.App.WebApi.";
        private const string HttpConfigurationDataKey = "HttpConfiguration";

        public static TrendyolAppBuilder UseWebApi(this TrendyolAppBuilder builder, IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("Default", "{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.Clear();
            config.Formatters.Add(CreateJsonFormatter());
            config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            app.UseWebApi(config);

            builder.SetData(TrendyolAppDataKeyPrefix + HttpConfigurationDataKey, config);
            return builder;
        }

        public static TrendyolAppBuilder UseHttpsGuard(this TrendyolAppBuilder builder, IAppBuilder app)
        {
            HttpConfiguration config = builder.GetData<HttpConfiguration>(TrendyolAppDataKeyPrefix + HttpConfigurationDataKey);

            if (config == null)
            {
                throw new ConfigurationErrorsException("You must register your app with UseWebApi method before calling UseHttpsGuard.");
            }

            config.MessageHandlers.Add(new HttpsGuard());

            return builder;
        }

        private static JsonMediaTypeFormatter CreateJsonFormatter()
        {
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            };

            return formatter;
        }
    }
}
