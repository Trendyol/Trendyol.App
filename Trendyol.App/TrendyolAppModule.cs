using System;

namespace Trendyol.App
{
    public abstract class TrendyolAppModule : ITrendyolAppBuilder
    {
        private readonly TrendyolAppBuilder _builder;

        protected TrendyolAppModule(TrendyolAppBuilder builder1)
        {
            _builder = builder1;
        }

        public void BeforeBuild(Action action)
        {
            _builder.BeforeBuild(action);
        }

        public TrendyolApp Build()
        {
            return _builder.Build();
        }

        public void AfterBuild(Action action)
        {
            _builder.AfterBuild(action);
        }

        public TrendyolAppBuilder Then()
        {
            return _builder;
        }
    }
}