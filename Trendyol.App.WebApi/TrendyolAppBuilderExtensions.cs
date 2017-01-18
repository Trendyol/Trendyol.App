using System;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;
using Trendyol.App.WebApi.Handlers;

namespace Trendyol.App.WebApi
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolWebApiBuilder UseWebApi(this TrendyolAppBuilder builder, IAppBuilder app, string applicationName, string customSwaggerContentPath = null)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            builder.BeforeBuild(() =>
            {
                HttpConfiguration config = new HttpConfiguration();

                config.EnableSwagger("docs/{apiVersion}/swagger", c =>
                {
                    c.SingleApiVersion("v1", applicationName)
                     .Description($"{applicationName} documentation.");
                })
                .EnableSwaggerUi("help/{*assetPath}", c =>
                {
                    if (!String.IsNullOrEmpty(customSwaggerContentPath))
                    {
                        c.InjectJavaScript(callingAssembly, $"{callingAssembly.GetName().Name}.{customSwaggerContentPath}");
                    }

                    c.DisableValidator();
                });

                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute("Default", "{controller}/{id}", new { id = RouteParameter.Optional });

                config.Formatters.Clear();
                config.Formatters.Add(CreateJsonFormatter());
                config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
                config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

                builder.DataStore.SetData(Constants.HttpConfigurationDataKey, config);
            });

            builder.AfterBuild(() =>
            {
                HttpConfiguration config = builder.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);
                app.UseWebApi(config);
            });

            return new TrendyolWebApiBuilder(builder, app, applicationName);
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