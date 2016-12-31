using System;
using System.Reflection;
using Autofac;
using Trendyol.App.Data;
using Trendyol.App.Domain;

namespace Trendyol.App.Autofac
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofac(this TrendyolAppBuilder builder, Action<ContainerBuilder> action = null, Assembly serviceAssembly = null, Assembly dataAssembly = null)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            if (serviceAssembly != null)
            {
                containerBuilder
                    .RegisterAssemblyTypes(serviceAssembly)
                    .Where(item => item.Implements(typeof(IService)) && item.IsAbstract == false)
                    .AsImplementedInterfaces()
                    .SingleInstance();
            }

            if (dataAssembly != null)
            {
                containerBuilder
                    .RegisterAssemblyTypes(dataAssembly)
                    .Where(item => item.Implements(typeof(IRepository)) && item.IsAbstract == false)
                    .AsImplementedInterfaces()
                    .SingleInstance();
            }

            action?.Invoke(containerBuilder);

            IContainer container = containerBuilder.Build();

            builder.SetData(Constants.AutofacContainerDataKey, container);

            return builder;
        }
    }
}
