using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Trendyol.App.Data;
using Trendyol.App.Data.Attributes;

namespace Trendyol.App.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static TId GetNextId<TEntity, TId>(this DbContext dbContext) where TEntity : IEntity<TId>
        {
            Type entityType = typeof(TEntity);

            SequenceAttribute attribute = entityType.GetCustomAttribute<SequenceAttribute>();

            if (attribute == null)
            {
                throw new InvalidOperationException("You need to decorate your entity with Sequence attribute to use this extension method.");
            }

            string sequenceName = attribute.Name;

            DbRawSqlQuery<TId> result = dbContext.Database.SqlQuery<TId>($"SELECT (NEXT VALUE FOR {sequenceName})");

            TId id = result.FirstOrDefault();

            if (id == null || id.Equals(default(TId)))
            {
                throw new InvalidOperationException($"Database did not return an instance of identity for sequence {sequenceName}.");
            }

            return id;
        }
    }
}