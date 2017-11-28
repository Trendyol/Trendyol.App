using Autofac;
using Trendyol.App;
using Trendyol.App.Autofac;
using Trendyol.App.BackgroundProcessing;
using Trendyol.App.Daemon;
using Trendyol.App.NLog;

namespace WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            TrendyolAppBuilder.Instance
                .UseAutofac(RegisterDependencies)
                .UseNLog()
                .UseDaemon<SampleWindowsService>("SampleWindowsService")
                .Build();
        }

       static void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TestJob>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
