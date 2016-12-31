using System.Collections.Generic;

namespace Trendyol.App
{
    public class TrendyolApp : InMemoryDataStore
    {
        public static TrendyolApp Instance { get; set; }

        public TrendyolApp(Dictionary<string, object> data)
        {
            _data = data;
        }
    }
}
