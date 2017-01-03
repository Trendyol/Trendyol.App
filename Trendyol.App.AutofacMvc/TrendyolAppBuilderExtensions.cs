using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Trendyol.App.Autofac;
using Trendyol.App.Domain;
using Trendyol.App.Mvc.ControllerHandlers;

namespace Trendyol.App.AutofacMvc
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofacMvc(this TrendyolAppBuilder builder, Assembly controllerAssembly)
        {
            IContainer container = builder.GetData<IContainer>(Constants.AutofacContainerDataKey);

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder
                    .RegisterAssemblyTypes(controllerAssembly)
                    .Where(item => item.Implements(typeof(IControllerHandler)) && item.IsAbstract == false)
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
            containerBuilder.RegisterControllers(controllerAssembly);
            containerBuilder.Update(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return builder;
        }
    }
}
