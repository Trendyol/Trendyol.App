using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using EntityFramework.InterceptorEx;

namespace Trendyol.App.EntityFramework
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolDataContextBuilder<T> UseDataContext<T>(this TrendyolAppBuilder builder) where T : DataContextBase
        {
            builder.BeforeBuild(() =>
            {
                Database.SetInitializer<T>(null);
                var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
                DbInterception.Add(new WithNoLockInterceptor());
            });

            return new TrendyolDataContextBuilder<T>(builder);
        }
    }
}
