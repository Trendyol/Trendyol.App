using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Trendyol.App.EntityFramework.Extensions
{
    public static class LinqExtensions
    {
         private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        private const string Wildcard = "*";

        private static Expression<Func<TSource, bool>> LikeExpression<TSource, TMember>(Expression<Func<TSource, TMember>> property, string value)
        {
            var param = Expression.Parameter(typeof(TSource), "t");
            var member = GetMemberExpression(property, param);

            var startWith = value.StartsWith(Wildcard);
            var endsWith = value.EndsWith(Wildcard);

            var originalTerm = value;

            if (startWith)
            {
                originalTerm = originalTerm.Remove(0, 1);
            }

            if (endsWith)
            {
                originalTerm = originalTerm.Remove(originalTerm.Length - 1, 1);
            }

            var constant = Expression.Constant(originalTerm);
            Expression exp;

            if (endsWith && startWith)
            {
                exp = Expression.Call(member, ContainsMethod, constant);
            }
            else if (startWith)
            {
                exp = Expression.Call(member, EndsWithMethod, constant);
            }
            else if (endsWith)
            {
                exp = Expression.Call(member, StartsWithMethod, constant);
            }
            else
            {
                exp = Expression.Equal(member, constant);
            }

            return Expression.Lambda<Func<TSource, bool>>(exp, param);
        }

        public static IQueryable<TSource> Like<TSource, TMember>(this IQueryable<TSource> source, Expression<Func<TSource, TMember>> parameter, string value)
        {
            return source.Where(LikeExpression(parameter, value));
        }

        public static Expression GetMemberExpression(Expression expression, Expression parentExpression)
        {
            var memberExpr = CastMemberExpression(expression);

            return GetMemberExpressionChain(memberExpr, parentExpression);
        }

        private static Expression GetMemberExpressionChain(Expression currentExpression, Expression parentExpression)
        {
            if (currentExpression is ParameterExpression)
            {
                return parentExpression;
            }

            // ReSharper disable once UsePatternMatching
            var memberExpression = currentExpression as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Member expression is required");
            }

            var newParentExpression = GetMemberExpressionChain(memberExpression.Expression, parentExpression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Only properties are supported: {memberExpression.Member.Name}");
            }

            var chainedExpression = Expression.Property(newParentExpression, (PropertyInfo)memberExpression.Member);
            return chainedExpression;
        }

        private static MemberExpression CastMemberExpression(Expression expression)
        {
            var lambda = expression as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            MemberExpression memberExpr = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                {
                    memberExpr = ((UnaryExpression) lambda.Body).Operand as MemberExpression;
                    break;
                }
                case ExpressionType.MemberAccess:
                {
                    memberExpr = lambda.Body as MemberExpression;
                    break;
                }
                default:
                {
                    throw new InvalidOperationException(
                        "Specified expression is invalid. Unable to determine property info from expression.");
                }
            }

            return memberExpr;
        }
    }
}