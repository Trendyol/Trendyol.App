using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Trendyol.App.WebApi.Handlers;
using Trendyol.App.WebApi.HealthCheck;

namespace Trendyol.App.WebApi
{
    public class TrendyolWebApiBuilder : TrendyolAppModule
    {
        private readonly IAppBuilder _owinBuilder;
        private readonly TrendyolAppBuilder _appBuilder;
        private readonly string _applicationName;

        public TrendyolWebApiBuilder(TrendyolAppBuilder builder, IAppBuilder owinBuilder, string applicationName)
            : base(builder)
        {
            _appBuilder = builder;
            _applicationName = applicationName;
            _owinBuilder = owinBuilder;
        }

        public TrendyolWebApiBuilder WithHttpsGuard()
        {
            _appBuilder.BeforeBuild(() =>
            {
                HttpConfiguration config = _appBuilder.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);

                if (config == null)
                {
                    throw new ConfigurationErrorsException(
                        "You must register your app with UseWebApi method before calling UseHttpsGuard.");
                }

                config.MessageHandlers.Add(new HttpsGuard());
            });

            return this;
        }

        public TrendyolWebApiBuilder WithCors(CorsOptions corsOptions)
        {
            _appBuilder.BeforeBuild(() =>
            {
                _owinBuilder.UseCors(corsOptions);
            });

            return this;
        }

        public TrendyolWebApiBuilder AsOAuthServer(OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions)
        {
            _appBuilder.BeforeBuild(() =>
            {
                _owinBuilder.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            });

            return this;
        }

        public TrendyolWebApiBuilder WithOAuth(OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions)
        {
            _appBuilder.BeforeBuild(() =>
            {
                _owinBuilder.UseOAuthBearerAuthentication(oAuthBearerAuthenticationOptions);
            });

            return this;
        }

        public TrendyolWebApiBuilder WithLanguages(params string[] supportedLanguages)
        {
            _appBuilder.BeforeBuild(() =>
            {
                HttpConfiguration config = _appBuilder.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);

                if (config == null)
                {
                    throw new ConfigurationErrorsException(
                        "You must register your app with UseWebApi method before calling UseHttpsGuard.");
                }

                if (supportedLanguages.IsEmpty())
                {
                    throw new ConfigurationErrorsException(
                        "You must add at least 1 language to use localization support.");
                }

                config.MessageHandlers.Insert(1, new LanguageHandler(supportedLanguages.ToList()));
            });

            return this;
        }

        public TrendyolWebApiBuilder WithMediaTypeFormatters(params MediaTypeFormatter[] formatters)
        {
            _appBuilder.BeforeBuild(() =>
            {
                HttpConfiguration config = _appBuilder.DataStore.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);

                if (config == null)
                {
                    throw new ConfigurationErrorsException(
                        "You must register your app with UseWebApi method before calling UseHttpsGuard.");
                }

                config.Formatters.Clear();

                foreach (MediaTypeFormatter formatter in formatters)
                {
                    config.Formatters.Add(formatter);
                }
            });

            return this;
        }

        public TrendyolWebApiBuilder WithHealthCheckerActivator(IHealthCheckerActivator activator)
        {
            _appBuilder.DataStore.SetData(Constants.HealthCheckerActivatorDataKey, activator);

            return this;
        }

        public TrendyolWebApiBuilder WithHealthChecker<T>() where T : IHealthChecker
        {
            HealthCheckerContainer container = _appBuilder.DataStore.GetData<HealthCheckerContainer>(Constants.HealthCheckerContainerDataKey);

            if (container == null)
            {
                container = new HealthCheckerContainer();
            }

            container.AddHealthChecker(typeof(T));

            _appBuilder.DataStore.SetData(Constants.HealthCheckerContainerDataKey, container);

            return this;
        }
    }
}