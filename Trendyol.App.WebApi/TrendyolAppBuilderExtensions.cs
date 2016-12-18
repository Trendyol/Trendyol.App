using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseWebApi(this TrendyolAppBuilder builder, IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("Default", "{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.Clear();
            config.Formatters.Add(CreateJsonFormatter());
            app.UseWebApi(config);

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
