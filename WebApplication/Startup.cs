using Microsoft.Owin;
using Owin;
using Trendyol.App;
using Trendyol.App.NLog;
using Trendyol.App.WebApi;
using WebApplication;

[assembly: OwinStartup(typeof(Startup))]
namespace WebApplication
{
    public class Startup
    {
        public static TrendyolApp App;

        public void Configuration(IAppBuilder app)
        {
            App = TrendyolAppBuilder.Instance
                .UseWebApi(app)
                .UseNLog()
                .Build();
        }
    }
}