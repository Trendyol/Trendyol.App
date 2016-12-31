using System.Reflection;
using Autofac;
using Data;
using Domain.Services;
using Microsoft.Owin;
using Owin;
using Trendyol.App;
using Trendyol.App.Autofac;
using Trendyol.App.AutofacWebApi;
using Trendyol.App.NLog;
using Trendyol.App.WebApi;
using WebApplication;

[assembly: OwinStartup(typeof(TrendyolApi))]
namespace WebApplication
{
    public class TrendyolApi
    {
        public void Configuration(IAppBuilder app)
        {
            TrendyolAppBuilder.Instance
                .UseWebApi(app, "Sample Api")
                .UseAutofac(RegisterDependencies, typeof(ISampleService).Assembly, typeof(DataContext).Assembly)
                .UseAutofacWebApi(Assembly.GetExecutingAssembly())
                .UseNLog()
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
        }
    }
}