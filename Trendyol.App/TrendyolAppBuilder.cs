using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Objects.DateTimeProviders;

namespace Trendyol.App
{
    public class TrendyolAppBuilder : ITrendyolAppBuilder
    {
        private static TrendyolAppBuilder _appBuilder;
        public static TrendyolAppBuilder Instance => _appBuilder ?? (_appBuilder = new TrendyolAppBuilder());

        private readonly List<Action> _beforeBuildTasks = new List<Action>();
        private readonly List<Action> _afterBuildTasks = new List<Action>();

        public InMemoryDataStore DataStore = new InMemoryDataStore();

        public TrendyolApp Build()
        {
            SetGlobalJsonSerializerSettings();

            foreach (var task in _beforeBuildTasks)
            {
                task.Invoke();
            }

            TrendyolApp.Instance = new TrendyolApp(DataStore);

            foreach (var task in _afterBuildTasks)
            {
                task.Invoke();
            }

            return TrendyolApp.Instance;
        }

        public void BeforeBuild(Action action)
        {
            _beforeBuildTasks.Add(action);
        }

        public void AfterBuild(Action action)
        {
            _afterBuildTasks.Add(action);
        }

        public TrendyolAppBuilder UseLocalTimes()
        {
            DataStore.SetData(Constants.DateTimeProvider, new LocalDateTimeProvider());

            return this;
        }

        public TrendyolAppBuilder UseUtcTimes()
        {
            DataStore.SetData(Constants.DateTimeProvider, new UtcDateTimeProvider());

            return this;
        }

        private void SetGlobalJsonSerializerSettings()
        {
            IDateTimeProvider dateTimeProvider = DataStore.GetData<IDateTimeProvider>(Constants.DateTimeProvider);

            if (dateTimeProvider != null && dateTimeProvider.Kind == DateTimeKind.Local)
            {
                TrendyolApp.JsonSerializerSettings = GetJsonSerializerSettings(DateTimeZoneHandling.Local);
            }
            else if (dateTimeProvider != null && dateTimeProvider.Kind == DateTimeKind.Utc)
            {
                TrendyolApp.JsonSerializerSettings = GetJsonSerializerSettings(DateTimeZoneHandling.Utc);
            }
            else
            {
                TrendyolApp.JsonSerializerSettings = GetJsonSerializerSettings();
            }
        }

        private JsonSerializerSettings GetJsonSerializerSettings(DateTimeZoneHandling dateTimeZoneHandling = DateTimeZoneHandling.Utc)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = dateTimeZoneHandling
            };

            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            return settings;
        }
    }
}
