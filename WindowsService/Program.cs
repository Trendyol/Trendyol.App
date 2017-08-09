using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Common.Logging;
using Topshelf;
using Trendyol.App;
using Trendyol.App.Aspects;
using Trendyol.App.Daemon;
using Trendyol.App.NLog;

namespace WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            TrendyolAppBuilder.Instance
                .UseNLog()
                .UseDaemon<SampleWindowsService>()
                .Build();
        }
    }
}
