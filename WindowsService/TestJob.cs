using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using RestSharp;
using Trendyol.App.BackgroundProcessing;
using Trendyol.App.RestClient;

namespace WindowsService
{
    public class TestJob : IJob
    {
        private static readonly ILog Logger = LogManager.GetLogger<TestJob>();

        public async Task Run()
        {
        }
    }
}