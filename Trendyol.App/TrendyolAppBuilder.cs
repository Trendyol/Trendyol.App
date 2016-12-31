namespace Trendyol.App
{
    public class TrendyolAppBuilder : InMemoryDataStore
    {
        private static TrendyolAppBuilder _appBuilder;

        public static TrendyolAppBuilder Instance => _appBuilder ?? (_appBuilder = new TrendyolAppBuilder());

        public TrendyolApp Build()
        {
            TrendyolApp.Instance = new TrendyolApp(_data);
            return TrendyolApp.Instance;
        }
    }
}
