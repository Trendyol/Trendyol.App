using System;
using System.Linq;
using System.Linq.Expressions;

namespace Trendyol.App.EntityFramework.Extensions
{
    public class QueryBuilder<TEntity> where TEntity : class
    {
         private IQueryable<TEntity> _query;

        public QueryBuilder(IQueryable<TEntity> query)
        {
            _query = query;
        }

        public static QueryBuilder<T> For<T>(IQueryable<T> queryable) where T : class
        {
            return new QueryBuilder<T>(queryable);
        }

        #region Equals

        public QueryBuilder<TEntity> Equals<TMember>(Expression<Func<TEntity, TMember?>> expression, TMember? value)
          where TMember : struct
        {
            if (value.HasValue)
            {
                WhereEquals(expression, value.Value);
            }

            return this;
        }

        public QueryBuilder<TEntity> Equals<TMember>(Expression<Func<TEntity, TMember>> expression, TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                WhereEquals(expression, value.Value);
            }

            return this;
        }

        public QueryBuilder<TEntity> Equals(Expression<Func<TEntity, string>> expression, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WhereEquals(expression, value);
            }

            return this;
        }

        private void WhereEquals<TMember>(
            Expression<Func<TEntity, TMember>> expression, TMember value)
            where TMember : struct
        {
            var constant = Expression.Constant(value);
            Where(Expression.Equal, expression, constant);
        }

        private void WhereEquals<TMember>(
            Expression<Func<TEntity, TMember?>> expression, TMember value)
            where TMember : struct
        {
            var constant = Expression.Constant(value);
            var nullableConstants = Expression.Convert(constant, typeof(TMember?));
            Where(Expression.Equal, expression, nullableConstants);
        }

        private void WhereEquals(
            Expression<Func<TEntity, string>> expression, string value)
        {
            var constant = Expression.Constant(value);
            Where(Expression.Equal, expression, constant);
        }
        #endregion

        #region Strings Functions

        public QueryBuilder<TEntity> Like<TMember>(Expression<Func<TEntity, TMember>> expression, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _query = _query.Like(expression, value);
            }

            return this;
        }

        public QueryBuilder<TEntity> StartsWith<TMember>(Expression<Func<TEntity, TMember>> expression, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string likeValue = $"{value}*";
                _query = _query.Like(expression, likeValue);
            }

            return this;
        }

        #endregion

        #region GreaterThan

        public QueryBuilder<TEntity> GreaterThanOrEquals<TMember>(Expression<Func<TEntity, TMember>> expression,
           TMember? value)
           where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                Where(Expression.GreaterThanOrEqual, expression, constant);
            }

            return this;
        }

        public QueryBuilder<TEntity> GreaterThanOrEquals<TMember>(Expression<Func<TEntity, TMember?>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                var nullableConstants = Expression.Convert(constant, typeof(TMember?));
                Where(Expression.GreaterThanOrEqual, expression, nullableConstants);
            }

            return this;
        }

        public QueryBuilder<TEntity> GreaterThan<TMember>(Expression<Func<TEntity, TMember>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                Where(Expression.GreaterThan, expression, constant);
            }

            return this;
        }

        public QueryBuilder<TEntity> GreaterThan<TMember>(Expression<Func<TEntity, TMember?>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                var nullableConstants = Expression.Convert(constant, typeof(TMember?));
                Where(Expression.GreaterThan, expression, nullableConstants);
            }

            return this;
        }

        #endregion

        #region LessThan


        public QueryBuilder<TEntity> LessThanOrEquals<TMember>(Expression<Func<TEntity, TMember>> expression,
         TMember? value)
         where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                Where(Expression.LessThanOrEqual, expression, constant);
            }

            return this;
        }

        public QueryBuilder<TEntity> LessThanOrEquals<TMember>(Expression<Func<TEntity, TMember?>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                var nullableConstants = Expression.Convert(constant, typeof(TMember?));
                Where(Expression.LessThanOrEqual, expression, nullableConstants);
            }

            return this;
        }

        public QueryBuilder<TEntity> LessThan<TMember>(Expression<Func<TEntity, TMember>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                Where(Expression.LessThan, expression, constant);
            }

            return this;
        }

        public QueryBuilder<TEntity> LessThan<TMember>(Expression<Func<TEntity, TMember?>> expression,
            TMember? value)
            where TMember : struct
        {
            if (value.HasValue)
            {
                var constant = Expression.Constant(value);
                var nullableConstants = Expression.Convert(constant, typeof(TMember?));
                Where(Expression.LessThan, expression, nullableConstants);
            }

            return this;
        }

        #endregion

        private void Where(Func<Expression, Expression, BinaryExpression> comparisionExpression,
            Expression member, Expression value)
        {
            var param = Expression.Parameter(typeof(TEntity), "t");
            var memberExpr = LinqExtensions.GetMemberExpression(member, param);
            var finalExpression = comparisionExpression(memberExpr, value);
            var whereExpression = Expression.Lambda<Func<TEntity, bool>>(finalExpression, param);
            _query = _query.Where(whereExpression);
        }

        public QueryBuilder<TEntity> Where<TMember>(Func<bool> when,
            Func<Expression, Expression, BinaryExpression> comparisionExpression, Expression<Func<TEntity, TMember>> member,
            TMember value)
        {
            if (when())
            {
                Where(comparisionExpression, member, Expression.Constant(value));
            }

            return this;
        }

        public IQueryable<TEntity> Queryable()
        {
            return _query;
        }

    }

    public class QueryBuilder
    {
        public static QueryBuilder<T> For<T>(IQueryable<T> queryable) where T : class
        {
            return new QueryBuilder<T>(queryable);
        }
    }
}