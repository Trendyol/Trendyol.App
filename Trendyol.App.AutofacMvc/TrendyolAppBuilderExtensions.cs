using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Trendyol.App.Mvc.ControllerHandlers;

namespace Trendyol.App.AutofacMvc
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofacMvc(this TrendyolAppBuilder builder, Assembly controllerAssembly)
        {
            builder.BeforeBuild(() =>
            {
                ContainerBuilder containerBuilder = builder.DataStore.GetData<ContainerBuilder>(Autofac.Constants.AutofacContainerBuilderDataKey);

                containerBuilder
                        .RegisterAssemblyTypes(controllerAssembly)
                        .Where(item => item.Implements(typeof(IControllerHandler)) && item.IsAbstract == false)
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

                containerBuilder.RegisterControllers(controllerAssembly);
            });

            builder.AfterBuild(() =>
            {
                IContainer container = builder.DataStore.GetData<IContainer>(Autofac.Constants.AutofacContainerDataKey);
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            });

            return builder;
        }
    }
}
