using System;
using System.Configuration;
using System.Linq;
using Couchbase;
using Couchbase.Configuration.Client;

namespace Trendyol.App.Couchbase
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolDataBucketBuilder<T> UseDataBucket<T>(this TrendyolAppBuilder builder, params string[] endpoints) where T : DataBucketBase
        {
            if (endpoints.IsEmpty())
            {
                throw new ConfigurationErrorsException("You need to provide at least 1 endpoint address to initialize Couchbase connection.");
            }

            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = endpoints.Select(e => new Uri(e)).ToList()
            });

            return new TrendyolDataBucketBuilder<T>(builder);
        }
    }
}