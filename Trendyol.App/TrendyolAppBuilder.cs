using System;
using System.Collections.Generic;

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
            InMemoryDataStore dataStore = new InMemoryDataStore();

            foreach (var task in _beforeBuildTasks)
            {
                task.Invoke();
            }

            TrendyolApp.Instance = new TrendyolApp(dataStore);

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
    }
}
