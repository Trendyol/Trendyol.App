using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Linq.Extensions;
using Trendyol.App;
using Trendyol.App.Couchbase;
using Trendyol.App.Couchbase.Extensions;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Requests;

namespace CouchbaseClient
{
    class Program
    {
        public static BeerSampleDataBucket Bucket;

        static void Main(string[] args)
        {
            TrendyolAppBuilder.Instance
                .UseDataBucket<BeerSampleDataBucket>("http://localhost:8091")
                .Build();

            var request = new PagedRequest()
            {
                PageSize = 2,
                Page = 2,
                OrderBy = "id",
                Fields = "id,name",
                Order = OrderType.Desc
            };

            Bucket = new BeerSampleDataBucket();
            var beer = Bucket.Beers.ToPage(request);
            var beers = GetBeersAsync().Result;
        }

        private static async Task<List<Beer>> GetBeersAsync()
        {
            IEnumerable<Beer> asyncBeer = await Bucket.Beers.Where(b => b.Name == "21A IPA").ExecuteAsync();
            return asyncBeer.ToList();
        }
    }
}
