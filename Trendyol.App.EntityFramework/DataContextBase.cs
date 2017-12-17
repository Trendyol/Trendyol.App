using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using EntityFramework.DynamicFilters;
using EntityFramework.InterceptorEx;
using Trendyol.App.Data;
using Trendyol.App.Data.Attributes;
using Trendyol.App.EntityFramework.Mapping;

namespace Trendyol.App.EntityFramework
{
    public abstract class DataContextBase : DbContext
    {
        private static readonly ILog Logger = LogManager.GetLogger<DataContextBase>();
        protected bool SetUpdatedOnSameAsCreatedOnForNewObjects { get; set; }

        public DataContextBase(string connectionStringName, bool logExecutedQueries = false)
            : base(connectionStringName)
        {
            InitializeContext(logExecutedQueries);
        }

        public DataContextBase(DbConnection existingConnection, bool contextOwnsConnection, bool logExecutedQueries = false)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeContext(logExecutedQueries);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> typesToRegister = GetType().Assembly.GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(
                    type =>
                        type.BaseType != null && type.BaseType.IsGenericType &&
                        type.BaseType.GetGenericTypeDefinition() == typeof(DataEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                DataContextAttribute dataContextAttribute = type.GetCustomAttribute<DataContextAttribute>();

                if (dataContextAttribute == null || dataContextAttribute.Type == GetType())
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }

            modelBuilder.Filter("SoftDeleteFilter", (ISoftDeletable d) => d.IsDeleted, false);
        }

        public override int SaveChanges()
        {
            HandleSoftDeletableEntities();
            HandleAuditableEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            HandleSoftDeletableEntities();
            HandleAuditableEntities();

            return base.SaveChangesAsync();
        }

        private void HandleSoftDeletableEntities()
        {
            IEnumerable<DbEntityEntry> entries = ChangeTracker.Entries().Where(x => x.Entity is ISoftDeletable && x.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                ISoftDeletable entity = (ISoftDeletable)entry.Entity;
                entity.IsDeleted = true;
            }
        }

        private void HandleAuditableEntities()
        {
            string currentUser = Thread.CurrentPrincipal?.Identity?.Name;

            IEnumerable<DbEntityEntry> entities = ChangeTracker.Entries().Where(x => x.Entity is IAuditable && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (DbEntityEntry entry in entities)
            {
                if (entry.Entity is IAuditable)
                {
                    IAuditable auditable = ((IAuditable)entry.Entity);

                    if (entry.State == EntityState.Added)
                    {
                        if (auditable.CreatedOn == DateTime.MinValue)
                        {
                            auditable.CreatedOn = TrendyolApp.Instance.DateTimeProvider.Now;

                            if (SetUpdatedOnSameAsCreatedOnForNewObjects)
                            {
                                auditable.UpdatedOn = auditable.CreatedOn;
                            }
                        }

                        if (String.IsNullOrEmpty(auditable.CreatedBy))
                        {
                            auditable.CreatedBy = currentUser;
                        }
                    }
                    else
                    {
                        auditable.UpdatedOn = TrendyolApp.Instance.DateTimeProvider.Now;
                        auditable.UpdatedBy = currentUser;
                    }
                }
            }
        }

        public virtual TId GetNextId<TEntity, TId>() where TEntity : IEntity<TId>
        {
            Type entityType = typeof(TEntity);

            SequenceAttribute attribute = entityType.GetCustomAttribute<SequenceAttribute>();

            if (attribute == null)
            {
                throw new InvalidOperationException("You need to decorate your entity with Sequence attribute to use this extension method.");
            }

            string sequenceName = attribute.Name;

            DbRawSqlQuery<TId> result = Database.SqlQuery<TId>($"SELECT (NEXT VALUE FOR {sequenceName})");

            TId id = result.FirstOrDefault();

            if (id == null || id.Equals(default(TId)))
            {
                throw new InvalidOperationException($"Database did not return an instance of identity for sequence {sequenceName}.");
            }

            return id;
        }

        private void InitializeContext(bool logExecutedQueries)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            if (logExecutedQueries)
            {
                Database.Log = Logger.Trace;
            }
        }
    }
}
