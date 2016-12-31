using System;
using System.Reflection;
using Autofac;
using Trendyol.App.Data;
using Trendyol.App.Domain;

namespace Trendyol.App.Autofac
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofac(this TrendyolAppBuilder builder, Assembly serviceAssembly, Assembly dataAssembly, Action<ContainerBuilder> action)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyTypes(serviceAssembly)
                .Where(item => item.Implements(typeof(IService)) && item.IsAbstract == false)
                .AsImplementedInterfaces()
                .SingleInstance();

            containerBuilder
                .RegisterAssemblyTypes(dataAssembly)
                .Where(item => item.Implements(typeof(IRepository)) && item.IsAbstract == false)
                .AsImplementedInterfaces()
                .SingleInstance();

            action(containerBuilder);

            IContainer container = containerBuilder.Build();

            builder.SetData(Constants.AutofacContainerDataKey, container);

            return builder;
        }
    }
}
