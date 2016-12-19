using System.Collections.Generic;

namespace Trendyol.App
{
    public class TrendyolApp : InMemoryDataStore
    {
        public TrendyolApp(Dictionary<string, object> data)
        {
            _data = data;
        }
    }
}
