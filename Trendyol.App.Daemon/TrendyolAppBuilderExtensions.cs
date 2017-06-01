using System;
using System.Linq;
using Common.Logging;
using Topshelf;
using Topshelf.Common.Logging;

namespace Trendyol.App.Daemon
{
    public static class TrendyolAppBuilderExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger("Trendyol.App.Daemon.HostFactory");

        public static TrendyolAppBuilder UseDaemon<T>(this TrendyolAppBuilder appBuilder,
            string serviceName,
            int defaultStartStopTimeoutInMinutes = 3,
            ServiceAccountType serviceAccountType = ServiceAccountType.LocalSystem,
            string serviceAccountUserName = "",
            string serviceAccountPassword = "",
            params string[] dependencies) where T : TrendyolWindowsService
        {
            appBuilder.AfterBuild(() =>
            {
                HostFactory.Run(x =>
                {
                    x.Service<T>(s =>
                    {
                        s.ConstructUsing(srv => Activator.CreateInstance<T>());
                        s.WhenStarted(srv => srv.Start());
                        s.WhenStopped(srv => srv.Stop());
                    });

                    x.UseCommonLogging();

                    x.OnException((ex) =>
                    {
                        Logger.Error($"There was a problem executing {serviceName}.", ex);
                    });

                    x.StartAutomatically();
                    x.SetStartTimeout(TimeSpan.FromMinutes(defaultStartStopTimeoutInMinutes));
                    x.SetStopTimeout(TimeSpan.FromMinutes(defaultStartStopTimeoutInMinutes));
                    x.SetDescription(serviceName);
                    x.SetDisplayName(serviceName);
                    x.SetServiceName(serviceName);

                    if (dependencies != null && dependencies.Any())
                    {
                        foreach (string dependency in dependencies)
                        {
                            x.AddDependency(dependency);
                        }
                    }

                    switch (serviceAccountType)
                    {
                        case ServiceAccountType.LocalSystem:
                            x.RunAsLocalSystem();
                            break;
                        case ServiceAccountType.Custom:
                            if (String.IsNullOrEmpty(serviceAccountUserName))
                            {
                                throw new ArgumentException("ServiceAccountUserName cannot be null when account type is set to custom.", nameof(serviceAccountUserName));
                            }

                            if (String.IsNullOrEmpty(serviceAccountPassword))
                            {
                                throw new ArgumentException("ServiceAccountPassword cannot be null when account type is set to custom.", nameof(serviceAccountPassword));
                            }

                            x.RunAs(serviceAccountUserName, serviceAccountPassword);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(serviceAccountType), serviceAccountType, null);
                    }
                });
            });

            return appBuilder;
        }
    }
}
