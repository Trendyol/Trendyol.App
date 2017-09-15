using System.Linq;
using Trendyol.App.Couchbase;

namespace CouchbaseClient
{
    public class BeerSampleDataBucket : DataBucketBase
    {
        public BeerSampleDataBucket()
            : base("beer-sample", "12345678")
        {
        }

        public IQueryable<Beer> Beers => Query<Beer>();
    }
}