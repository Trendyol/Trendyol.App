using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using Trendyol.App;
using Trendyol.App.Daemon;

namespace WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            TrendyolAppBuilder.Instance
                .UseDaemon<SampleWindowsService>("SampleWindowsService")
                .Build();
        }
    }
}
