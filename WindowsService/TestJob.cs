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