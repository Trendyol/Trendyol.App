### Trendyol.App
This is the core library for building apps with this framework.
It includes most of abstractions and a simple thread runner.
To initialize Trendyol.App you need to build an application instance as follows:
```csharp
TrendyolAppBuilder.Instance
                .Build();
```
#### Features:
Trendyol.App has builtin support for logging via [CommonLogging](https://github.com/net-commons/common-logging) infrastructure.
You can initialize a logger as follows:
```csharp
using System;
using System.Threading;
using Common.Logging;
using Trendyol.App.BackgroundProcessing;

namespace WindowsService
{
    public class TestJob : IJob
    {
        private static readonly ILog Logger = LogManager.GetLogger<TestJob>();

        public void Run()
        {
            Logger.Info($"Example info text.");
            Console.WriteLine($"Job:{typeof(TestJob).FullName} running.");
            Thread.Sleep(10000);
            Console.WriteLine($"Job:{typeof(TestJob).FullName} finished.");
        }
    }
}
```
There is a configuration interface for reading configuration parameters from files or any other sources you care to implement.
```csharp
namespace Trendyol.App.Configuration
{
    public interface IConfigManager
    {
        string Get(string key);

        T Get<T>(string key);
    }
}
```
You can access current configuration provider from TrendyolApp instance.
```csharp
string value = TrendyolApp.Instance.ConfigManager.Get("test-key");
```
TODO: There will be an extension method to override default config provider which is reading data from app.config file.
