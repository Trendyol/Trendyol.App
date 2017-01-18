namespace Trendyol.App
{
    public class TrendyolApp
    {
        public static TrendyolApp Instance { get; set; }

        public InMemoryDataStore DataStore { get; set; }

        public TrendyolApp(InMemoryDataStore dataStore)
        {
            DataStore = dataStore;
        }
    }
}
