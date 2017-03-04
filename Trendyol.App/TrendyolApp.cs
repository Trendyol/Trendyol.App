using Trendyol.App.Configuration;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Objects.DateTimeProviders;

namespace Trendyol.App
{
    public class TrendyolApp
    {
        public static TrendyolApp Instance { get; set; }

        public InMemoryDataStore DataStore { get; set; }

        private IDateTimeProvider _dateTimeProvider;
        public IDateTimeProvider DateTimeProvider
        {
            get
            {
                if (_dateTimeProvider == null)
                {
                    _dateTimeProvider = DataStore.GetData<IDateTimeProvider>(Constants.DateTimeProvider);

                    if (_dateTimeProvider == null)
                    {
                        _dateTimeProvider = new UtcDateTimeProvider();
                    }
                }

                return _dateTimeProvider;
            }
        }

        private IConfigManager _configManager;
        public IConfigManager ConfigManager
        {
            get
            {
                if (_configManager == null)
                {
                    _configManager = DataStore.GetData<IConfigManager>(Constants.ConfigManager);

                    if (_configManager == null)
                    {
                        _configManager = new DefaultConfigManager();
                    }
                }

                return _configManager;
            }
        }

        public TrendyolApp(InMemoryDataStore dataStore)
        {
            DataStore = dataStore;
        }
    }
}
