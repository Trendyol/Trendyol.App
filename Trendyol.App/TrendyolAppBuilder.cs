namespace Trendyol.App
{
    public class TrendyolAppBuilder : InMemoryDataStore
    {
        private static TrendyolAppBuilder _appBuilder;
        public static TrendyolAppBuilder Instance => _appBuilder ?? new TrendyolAppBuilder();

        public TrendyolApp Build()
        {
            return new TrendyolApp(_data);
        }
    }
}
