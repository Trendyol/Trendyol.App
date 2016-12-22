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
using WebApplication.Data;
using WebApplication.Managers;
using WebApplication.Repositories;

[assembly: OwinStartup(typeof(TrendyolApi))]
namespace WebApplication
{
    public class TrendyolApi
    {
        public static TrendyolApp Instance;

        public void Configuration(IAppBuilder app)
        {
            Instance = TrendyolAppBuilder.Instance
                .UseWebApi(app, "Sample Api")
                .UseAutofac(RegisterDependencies)
                .UseAutofacWebApi(Assembly.GetExecutingAssembly())
                .UseNLog()
                .Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.Register(c => new Context()).InstancePerLifetimeScope();
            builder.RegisterType<SampleManager>().As<ISampleManager>().InstancePerLifetimeScope();
            builder.RegisterType<SampleRepository>().As<ISampleRepository>().InstancePerLifetimeScope();
        }
    }
}