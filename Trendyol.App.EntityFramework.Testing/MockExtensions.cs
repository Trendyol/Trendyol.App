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
            where TContext : DataContextBase<TContext>
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
    }
}
