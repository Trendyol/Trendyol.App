using System.Reflection;
using Autofac;
using Microsoft.Owin;
using Owin;
using Trendyol.App;
using Trendyol.App.Autofac;
using Trendyol.App.AutofacWebApi;
using Trendyol.App.NLog;
using Trendyol.App.WebApi;
using WebApplication;
using WebApplication.Managers;

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
                .UseAutofac(RegisterDependencies)
                .UseAutofacWebApi(Assembly.GetExecutingAssembly())
                .UseNLog()
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<SampleManager>().As<ISampleManager>().InstancePerLifetimeScope();
        }
    }
}