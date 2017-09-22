using System;
using System.Threading;
using Common.Logging;
using RestSharp;
using Trendyol.App.BackgroundProcessing;
using Trendyol.App.RestClient;

namespace WindowsService
{
    public class TestJob : IJob
    {
        private static readonly ILog Logger = LogManager.GetLogger<TestJob>();

        public void Run()
        {
            var restClient = new TrendyolRestClient("http://devxeonreceivingapi.trendyol.com", "Xeon.StockService", "topsecret", "http://devxeonauth.trendyol.com/connect/token", "receiving");

            var request = new RestRequest("receiving-order-items/{id}");
            request.AddUrlSegment("id", "1830743");

            var getResponse = restClient.Get<GetReceivingOrderItemResponse>(request);

            Logger.Info($"Example info text.");
            Console.WriteLine($"Job:{typeof(TestJob).FullName} running.");
            Thread.Sleep(10000);
            Console.WriteLine($"Job:{typeof(TestJob).FullName} finished.");
        }
    }
}