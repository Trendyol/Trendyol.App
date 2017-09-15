using System.Linq;
using Couchbase;
using Couchbase.Linq;

namespace Trendyol.App.Couchbase
{
    public class DataBucketBase : BucketContext
    {
        public DataBucketBase(string bucketName, string password = null)
            : this(ClusterHelper.GetBucket(bucketName, password))
        {
        }

        public DataBucketBase(global::Couchbase.Core.IBucket bucket) : 
            base(bucket)
        {
        }
    }
}