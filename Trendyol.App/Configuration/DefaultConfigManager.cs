using System;
using System.Configuration;

namespace Trendyol.App.Configuration
{
    public class DefaultConfigManager : IConfigManager
    {
        public string Get(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return ConfigurationManager.AppSettings.Get(key);
        }

        public T Get<T>(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string value = Get(key);

            if (String.IsNullOrEmpty(value))
            {
                return default(T);
            }

            T typedValue = (T)Convert.ChangeType(value, typeof(T));

            if (typedValue == null)
            {
                return default(T);
            }

            return typedValue;
        }
    }
}