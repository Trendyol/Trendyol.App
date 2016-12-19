using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Trendyol.App.AutofacWebApi
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofacWebApi(this TrendyolAppBuilder builder, Assembly controllerAssembly)
        {
            HttpConfiguration config = builder.GetData<HttpConfiguration>(Constants.HttpConfigurationDataKey);
            IContainer container = builder.GetData<IContainer>(Constants.AutofacContainerDataKey);

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(controllerAssembly);
            containerBuilder.Update(container);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return builder;
        }
    }
}
