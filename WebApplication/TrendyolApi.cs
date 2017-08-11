using System.Reflection;
using Autofac;
using Data;
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
                    //.WithDeepLogging(new SqlDeepLogger("SampleDataContext", "TestLogs"))
                    .WithBasicAuth(new AuthenticationChecker(), new UserStore())
                    .Then()
                .UseAutofac(RegisterDependencies, false, typeof(ISampleService).Assembly, typeof(SampleDataContext).Assembly)
                .UseAutofacWebApi(Assembly.GetExecutingAssembly())
                .UseNLog()
                .UseDataContext<SampleDataContext>()
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
        }
    }
}