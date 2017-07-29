using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Domain.Requests;
using NUnit.Framework;
using WebApplication.Controllers;

namespace Tests
{
    public class SampleControllerTests
    {
        [Test, Ignore("Integration Test")]
        public void Get_ShouldReturnEntity_WhenIdAndNameRequested()
        {
            SampleController controller = new SampleController(null, null, null);

            Stopwatch watch = Stopwatch.StartNew();

            var request = new QuerySamplesRequest()
            {
                Fields = "id,name"
            };

            for (int i = 0; i < 20000; i++)
            {
                var result = controller.Filter(request);
            }

            long elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsed);
        }       
    }
}