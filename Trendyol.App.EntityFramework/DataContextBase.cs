using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using EntityFramework.Filters;
using Trendyol.App.Data;

namespace Trendyol.App.EntityFramework
{
    public abstract class DataContextBase<T> : DbContext where T : DbContext
    {
        static DataContextBase()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<T>(null);
            DbInterception.Add(new FilterInterceptor());
        }

        public DataContextBase(string connectionStringName)
            : base(connectionStringName)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            this.EnableFilter("SoftDeleteFilter");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> typesToRegister = typeof(T).Assembly.GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace)).Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DataEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            modelBuilder.Conventions.Add(FilterConvention.Create<ISoftDeletable>("SoftDeleteFilter", (e) => e.IsDeleted == false));
        }
    }
}
