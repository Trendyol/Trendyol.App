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
            builder.BeforeBuild(() =>
            {
                ContainerBuilder containerBuilder = builder.DataStore.GetData<ContainerBuilder>(Autofac.Constants.AutofacContainerBuilderDataKey);
                containerBuilder.RegisterApiControllers(controllerAssembly);
            });

            builder.AfterBuild(() =>
            {
                HttpConfiguration config = builder.DataStore.GetData<HttpConfiguration>(WebApi.Constants.HttpConfigurationDataKey);
                IContainer container = builder.DataStore.GetData<IContainer>(Autofac.Constants.AutofacContainerDataKey);
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            });

            return builder;
        }
    }
}
