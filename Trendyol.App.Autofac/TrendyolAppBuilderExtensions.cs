using System;
using Autofac;

namespace Trendyol.App.Autofac
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofac(this TrendyolAppBuilder builder, Action<ContainerBuilder> action)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            action(containerBuilder);

            IContainer container = containerBuilder.Build();

            builder.SetData(Constants.AutofacContainerDataKey, container);

            return builder;
        }
    }
}
