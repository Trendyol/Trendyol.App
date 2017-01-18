using System;

namespace Trendyol.App
{
    public interface ITrendyolAppBuilder
    {
        void BeforeBuild(Action action);

        TrendyolApp Build();

        void AfterBuild(Action action);
    }
}