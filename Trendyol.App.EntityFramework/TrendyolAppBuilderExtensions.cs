using System;
using System.Data.Entity.Migrations;

namespace Trendyol.App.EntityFramework
{
    public static class TrendyolAppBuilderExtensions
    {
        public static TrendyolAppBuilder UseAutomaticMigrations<T>(this TrendyolAppBuilder builder) where T : DbMigrationsConfiguration
        {
            builder.AfterBuild(() =>
            {
                T configuration = Activator.CreateInstance<T>();
                DbMigrator migrator = new DbMigrator(configuration);
                migrator.Update();
            });

            return builder;
        }
    }
}
