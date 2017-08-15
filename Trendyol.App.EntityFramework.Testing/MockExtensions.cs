using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Moq;

namespace Trendyol.App.EntityFramework.Testing
{
    public static class MockExtensions
    {
        public static Mock<DbSet<TEntity>> SetupDbSet<TContext, TEntity>(this Mock<TContext> source,
            Expression<Func<TContext, DbSet<TEntity>>> dbSetExp, ICollection<TEntity> data)
            where TContext : DataContextBase
            where TEntity : class
        {
            var mockDbSet = new Mock<DbSet<TEntity>>().SetupData(data);

            mockDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                ParameterExpression peA = Expression.Parameter(typeof(TEntity), "d");
                Expression member = Expression.Property(peA, "Id");
                ConstantExpression constant = Expression.Constant(args.First());
                BinaryExpression body = Expression.Equal(member, constant);
                Expression<Func<TEntity, bool>> finalExpression = Expression.Lambda<Func<TEntity, bool>>(body, peA);
                return data.FirstOrDefault(finalExpression.Compile());
            });

            source.Setup(dbSetExp).Returns(mockDbSet.Object);

            return mockDbSet;
        }

        public static Mock<DbSet<TEntity>> SetupDbSet<TContext, TEntity>(this Mock<TContext> source,
            Expression<Func<TContext, DbSet<TEntity>>> dbSetExp)
            where TContext : DataContextBase
            where TEntity : class
        {
            return SetupDbSet(source, dbSetExp, new List<TEntity>());
        }

        public static IEnumerable<TEntity> SetupDbSet<TContext, TEntity>(this IEnumerable<TEntity> source,
            Expression<Func<TContext, DbSet<TEntity>>> dbSetExp, Mock<TContext> mockDataContext)
            where TContext : DataContextBase
            where TEntity : class
        {
            var data = source as IList<TEntity> ?? source.ToList();
            var mockDbSet = new Mock<DbSet<TEntity>>().SetupData(data.ToList());

            mockDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                ParameterExpression peA = Expression.Parameter(typeof(TEntity), "d");
                Expression member = Expression.Property(peA, "Id");
                ConstantExpression constant = Expression.Constant(args.First());
                BinaryExpression body = Expression.Equal(member, constant);
                Expression<Func<TEntity, bool>> finalExpression = Expression.Lambda<Func<TEntity, bool>>(body, peA);
                return data.FirstOrDefault(finalExpression.Compile());
            });

            mockDataContext.Setup(dbSetExp).Returns(mockDbSet.Object);

            return data;
        }

        public static TEntity SetupDbSet<TContext, TEntity>(this TEntity source,
            Expression<Func<TContext, DbSet<TEntity>>> dbSetExp, Mock<TContext> mockDataContext)
            where TContext : DataContextBase
            where TEntity : class
        {
            return new[] {source}.SetupDbSet(dbSetExp, mockDataContext).FirstOrDefault();
        }
    }
}