using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Trendyol.App.Data;
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

                if (isAllSystemSingleInstance)
                {
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

                        containerBuilder
                            .RegisterAssemblyTypes(dataAssembly)
                            .Where(item => item.Implements(typeof(IIdentityProvider)) && item.IsAbstract == false)
                            .AsImplementedInterfaces()
                            .SingleInstance();
                    }
                }
                else
                {
                    if (serviceAssembly != null)
                    {
                        containerBuilder
                            .RegisterAssemblyTypes(serviceAssembly)
                            .Where(item => item.Implements(typeof(IService)) && item.IsAbstract == false)
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();
                    }

                    if (dataAssembly != null)
                    {
                        containerBuilder
                            .RegisterAssemblyTypes(dataAssembly)
                            .Where(item => item.Implements(typeof(IRepository)) && item.IsAbstract == false)
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

                        containerBuilder
                            .RegisterAssemblyTypes(dataAssembly)
                            .Where(item => item.Implements(typeof(IIdentityProvider)) && item.IsAbstract == false)
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();
                    }
                }

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
    }
}
