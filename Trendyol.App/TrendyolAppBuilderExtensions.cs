using Trendyol.App.Domain.Objects.DateTimeProviders;

namespace Trendyol.App
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseLocalTimes(this TrendyolAppBuilder builder)
        {
            builder.BeforeBuild(() =>
            {
                builder.DataStore.SetData(Constants.DateTimeProvider, new LocalDateTimeProvider());
            });

            return builder;
        }

        public static TrendyolAppBuilder UseUtcTimes(this TrendyolAppBuilder builder)
        {
            builder.BeforeBuild(() =>
            {
                builder.DataStore.SetData(Constants.DateTimeProvider, new UtcDateTimeProvider());
            });

            return builder;
        }
    }
}