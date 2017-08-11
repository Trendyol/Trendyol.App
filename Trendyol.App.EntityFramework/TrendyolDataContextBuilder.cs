using System;
using System.Data.Entity.Migrations;

namespace Trendyol.App.EntityFramework
{
    public class TrendyolDataContextBuilder<T> : TrendyolAppModule where T : DataContextBase
    {
        private readonly TrendyolAppBuilder _appBuilder;

        public TrendyolDataContextBuilder(TrendyolAppBuilder builder) 
            : base(builder)
        {
            _appBuilder = builder;
        }

        public TrendyolDataContextBuilder<T> WithAutomaticMigrations<TConfig>() where TConfig : DbMigrationsConfiguration
        {
            _appBuilder.AfterBuild(() =>
            {
                TConfig configuration = Activator.CreateInstance<TConfig>();
                DbMigrator migrator = new DbMigrator(configuration);
                migrator.Update();
            });

            return this;
        }
    }
}