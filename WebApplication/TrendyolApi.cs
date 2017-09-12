using System.Reflection;
using Autofac;
using Common.Logging;
using Data;
using Data.Migrations;
using Domain.Services;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Trendyol.App;
using Trendyol.App.Autofac;
using Trendyol.App.AutofacWebApi;
using Trendyol.App.EntityFramework;
using Trendyol.App.NLog;
using Trendyol.App.WebApi;
using WebApplication;
using WebApplication.Authentication;
using WebApplication.HealthCheckers;

[assembly: OwinStartup(typeof(TrendyolApi))]
namespace WebApplication
{
    public class TrendyolApi
    {
        public void Configuration(IAppBuilder app)
        {
            TrendyolAppBuilder.Instance
                .UseLocalTimes()
                .UseWebApi(app, "Sample Api")
                    .WithCors(CorsOptions.AllowAll)
                    .WithHealthChecker<DatabaseHealthChecker>()
                    .WithLanguages("tr-TR")
                    .OnDisposing(Dispose)
                    .Then()
                .UseAutofac(RegisterDependencies, false, typeof(ISampleService).Assembly, typeof(SampleDataContext).Assembly)
                .UseAutofacWebApi(Assembly.GetExecutingAssembly())
                .UseDataContext<SampleDataContext>()
                    .WithAutomaticMigrations<Configuration>()
                    .Then()
                .UseNLog()
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
        }

        private void Dispose()
        {
            ILog logger = LogManager.GetLogger("Startup");
            logger.Debug("Disposing");
        }
    }
}