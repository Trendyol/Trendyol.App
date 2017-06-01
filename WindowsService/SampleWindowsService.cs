using System;
using System.Threading;
using Trendyol.App.BackgroundProcessing;
using Trendyol.App.Daemon;

namespace WindowsService
{
    public class SampleWindowsService : TrendyolWindowsService
    {
        public override void Start()
        {
            BackgroundJobManager.Register<TestJob>(5000);
            BackgroundJobManager.Start();
        }

        public override void Stop()
        {
            BackgroundJobManager.Stop();
        }
    }
}