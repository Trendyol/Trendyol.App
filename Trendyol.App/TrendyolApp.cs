using System.Collections.Generic;

namespace Trendyol.App
{
    public class TrendyolApp
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public void SetData(string key, object value)
        {
            _data.Add(key, value);
        }

        public object GetData(string key)
        {
            return _data.ContainsKey(key) ? _data[key] : null;
        }

        public T GetData<T>(string key)
        {
            return (T)GetData(key);
        }
    }
}
