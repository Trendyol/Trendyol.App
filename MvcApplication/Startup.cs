using System.Reflection;
using Autofac;
using Data;
using Domain.Services;
using Microsoft.Owin;
using MvcApplication;
using Owin;
using Trendyol.App;
using Trendyol.App.Autofac;
using Trendyol.App.AutofacMvc;
using Trendyol.App.Mvc;

[assembly: OwinStartup(typeof(Startup))]
namespace MvcApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            TrendyolAppBuilder.Instance
                .UseMvc(RouteConfig.RegisterRoutes)
                .UseAutofac(RegisterDependencies, true, typeof(ISampleService).Assembly, typeof(DataContext).Assembly)
                .UseAutofacMvc(Assembly.GetExecutingAssembly())
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
        }
    }
}