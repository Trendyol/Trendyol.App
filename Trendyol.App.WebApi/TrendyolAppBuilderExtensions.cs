using System;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using Newtonsoft.Json;
using Owin;
using Swashbuckle.Application;
using Trendyol.App.Domain.Abstractions;
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

                string rootUrl = builder.DataStore.GetData<string>(Constants.ApiRootUrlDataKey);

                config.EnableSwagger("docs/{apiVersion}/swagger", c =>
                {
                    c.SingleApiVersion("v1", applicationName)
                     .Description($"{applicationName} documentation.");
                    c.DescribeAllEnumsAsStrings(true);

                    if (!String.IsNullOrEmpty(rootUrl))
                    {
                        c.RootUrl(r => rootUrl);
                    }
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
                config.Formatters.Add(CreateJsonFormatter(builder));
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

        private static JsonMediaTypeFormatter CreateJsonFormatter(TrendyolAppBuilder builder)
        {
            IDateTimeProvider dateTimeProvider = builder.DataStore.GetData<IDateTimeProvider>(App.Constants.DateTimeProvider);

            JsonSerializerSettings settings;

            if (dateTimeProvider != null && dateTimeProvider.Kind == DateTimeKind.Local)
            {
                settings = TrendyolApp.GetJsonSerializerSettings(DateTimeZoneHandling.Local);
            }
            else if (dateTimeProvider != null && dateTimeProvider.Kind == DateTimeKind.Utc)
            {
                settings = TrendyolApp.GetJsonSerializerSettings(DateTimeZoneHandling.Utc);
            }
            else
            {
                settings = TrendyolApp.GetJsonSerializerSettings();
            }

            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings = settings
            };

            return formatter;
        }
    }
}