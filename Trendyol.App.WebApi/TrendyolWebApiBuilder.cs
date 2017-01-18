using System;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Trendyol.App.WebApi.Handlers;

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
    }
}