using System.Data.Entity.ModelConfiguration;

namespace Trendyol.App.EntityFramework
{
    public abstract class DataEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected DataEntityTypeConfiguration()
        {
            PostInitialize();
        }

        protected virtual void PostInitialize()
        {
        }
    }
}