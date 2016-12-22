using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Trendyol.App.EntityFramework
{
    public abstract class DataContextBase : DbContext
    {
        static DataContextBase()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<DataContextBase>(null);
        }

        public DataContextBase(string connectionStringName)
            : base(connectionStringName)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> typesToRegister = Assembly.GetCallingAssembly().GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace)).Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DataEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}
