using System;
using System.Threading;
using Microsoft.Owin.BuilderProperties;
using Owin;

namespace Trendyol.App.WebApi
{
    public static class AppBuilderExtensions
    {
        public static void BeforeDispose(this IAppBuilder app, Action cleanup)
        {
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(cleanup);
            }
        }
    }
}