using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Common.Logging;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Objects;
using Trendyol.App.Domain.Requests;

namespace Trendyol.App.EntityFramework.Extensions
{
    public static class QueryableExtensions
    {
        public static IPage<T> ToPage<T>(this IEnumerable<T> source, PagedRequest request)
        {
            if (source == null)
            {
                throw new ArgumentNullException($"You cannot pageinate on a null object reference. The parameter source should be initialized.", nameof(source));
            }

            if (request == null)
            {
                throw new ArgumentNullException($"You need to initialize a paging request before paging on a list. The parameter request should be initialized.", nameof(request));
            }

            if (request.Page == 0)
            {
                if (!String.IsNullOrEmpty(request.OrderBy))
                {
                    if (request.Order == OrderType.Asc)
                    {
                        source = source.OrderBy(request.OrderBy);
                    }
                    else
                    {
                        source = source.OrderBy(request.OrderBy + " descending");
                    }
                }

                return new Page<T>(source, 0, 0, 0);
            }

            if (String.IsNullOrEmpty(request.OrderBy))
            {
                throw new InvalidOperationException($"In order to use paging extensions you need to supply an OrderBy parameter.");
            }

            if (request.Order == OrderType.Asc)
            {
                source = source.OrderBy(request.OrderBy);
            }
            else
            {
                source = source.OrderBy(request.OrderBy + " descending");
            }

            int skip = (request.Page - 1) * request.PageSize;
            int take = request.PageSize;
            int totalItemCount = source.Count();

            return new Page<T>(source.Skip(skip).Take(take), request.Page, request.PageSize, totalItemCount);
        }

        public static IEnumerable<T> Select<T>(this IEnumerable<T> source, string fields) where T : class
        {
            // Check source.
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Check if we have any parameters.
            if (String.IsNullOrEmpty(fields))
            {
                return source;
            }

            // Create input parameter "o".
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "o");

            // Create new statement "new Data()".
            NewExpression newExpression = Expression.New(typeof(T));

            // Get a list of assignable members.
            IEnumerable<string> assignableMembers = fields
                .Split(',')
                .Where(o => typeof(T).GetProperty(o, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
                .Select(o => o.Trim());

            // Check if we have any assignable member.
            if (!assignableMembers.Any())
            {
                throw new ArgumentException($"None of the given names ({fields}) is assignable to a property on type: {typeof(T).FullName}", nameof(fields));
            }

            // Create initializers.
            IEnumerable<MemberAssignment> memberAssignments = assignableMembers
                .Select(o =>
                {
                 // Property "Field1".
                 PropertyInfo propertyInfo = typeof(T).GetProperty(o, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                 // Original value "o.Field1".
                 MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);

                 // Set value "Field1 = o.Field1".
                 return Expression.Bind(propertyInfo, memberExpression);
                });

            // Initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }".
            MemberInitExpression memberInitExpression = Expression.MemberInit(newExpression, memberAssignments);

            // Expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }".
            Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, parameterExpression);

            // Compile to Func<Data, Data>.
            return source.Select(lambda.Compile());
        }
    }
}