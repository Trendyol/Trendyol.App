using System;
using System.Threading;
using Trendyol.App.BackgroundProcessing;

namespace WindowsService
{
    public class TestJob : IJob
    {
        public void Run()
        {
            Console.WriteLine($"Job:{typeof(TestJob).FullName} running.");
            Thread.Sleep(10000);
            Console.WriteLine($"Job:{typeof(TestJob).FullName} finished.");
        }
    }
}