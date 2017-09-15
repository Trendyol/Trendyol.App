namespace Trendyol.App.Couchbase
{
    public class TrendyolDataBucketBuilder<T> : TrendyolAppModule where T : DataBucketBase
    {
        private readonly TrendyolAppBuilder _appBuilder;

        public TrendyolDataBucketBuilder(TrendyolAppBuilder builder) 
            : base(builder)
        {
            _appBuilder = builder;
        }
    }
}