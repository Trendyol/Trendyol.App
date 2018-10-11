using System;
using System.Reflection;
using Autofac;
using Trendyol.App.Configuration;
using Trendyol.App.Data;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Services;

namespace Trendyol.App.Autofac
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutofac(this TrendyolAppBuilder builder, Action<ContainerBuilder> action = null, bool isAllSystemSingleInstance = true, Assembly serviceAssembly = null, Assembly dataAssembly = null)
        {
            builder.BeforeBuild(() =>
            {
                ContainerBuilder containerBuilder = new ContainerBuilder();

                RegisterDependency<IService>(containerBuilder, serviceAssembly, isAllSystemSingleInstance);
                RegisterDependency<IRepository>(containerBuilder, dataAssembly, isAllSystemSingleInstance);
                RegisterDependency<IIdentityProvider>(containerBuilder, dataAssembly, isAllSystemSingleInstance);

                containerBuilder.Register<IConfigManager>(c => TrendyolApp.Instance.ConfigManager).SingleInstance();
                containerBuilder.Register<IDateTimeProvider>(c => TrendyolApp.Instance.DateTimeProvider).SingleInstance();
                action?.Invoke(containerBuilder);

                builder.DataStore.SetData(Constants.AutofacContainerBuilderDataKey, containerBuilder);
            });

            builder.AfterBuild(() =>
            {
                ContainerBuilder containerBuilder = builder.DataStore.GetData<ContainerBuilder>(Constants.AutofacContainerBuilderDataKey);
                IContainer container = containerBuilder.Build();
                builder.DataStore.SetData(Constants.AutofacContainerDataKey, container);
            });

            return builder;
        }

        private static void RegisterDependency<T>(ContainerBuilder containerBuilder, Assembly serviceAssembly, bool isAllSystemSingleInstance)
        {
            var dependency = containerBuilder
                .RegisterAssemblyTypes(serviceAssembly)
                .Where(item => item.Implements(typeof(T)) && item.IsAbstract == false)
                .AsImplementedInterfaces();

            if (isAllSystemSingleInstance)
            {
                dependency.SingleInstance();
            }
            else
            {
                dependency.InstancePerLifetimeScope();
            }
        }
    }
}
