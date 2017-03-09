using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Objects;
using Trendyol.App.Domain.Requests;
using Trendyol.App.EntityFramework.DynamicFiltering;

namespace Trendyol.App.EntityFramework.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

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

        public static IFilteredExpressionQuery<T> Select<T>(this IQueryable<T> source, string fields) where T : class
        {
            IEnumerable<string> selectedProperties = (fields ?? "").ToLowerInvariant().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

            // Take properties from the mapped entitiy that match selected properties
            IDictionary<string, PropertyInfo> sourceProperties = GetTypeProperties<T>(selectedProperties);

            // Construct runtime type by given property configuration
            Type runtimeType = _proxyGenerator.CreateClassProxy<T>().GetType();
            Type sourceType = typeof(T);

            // Create instance of source parameter
            ParameterExpression sourceParameter = Expression.Parameter(sourceType, "t");

            // Take fields from generated runtime type
            PropertyInfo[] runtimeTypeFields = runtimeType.GetProperties();

            // Elect selected fields if any.
            if (selectedProperties.Any())
            {
                runtimeTypeFields = runtimeTypeFields.Where(pi => selectedProperties.Any(p => pi.Name.ToLowerInvariant() == p)).ToArray();
            }

            // Generate bindings from source type to runtime type
            IEnumerable<MemberBinding> bindingsToRuntimeType = runtimeTypeFields
                .Select(field => Expression.Bind(
                    field,
                    Expression.Property(
                        sourceParameter,
                        sourceProperties[field.Name.ToLowerInvariant()]
                    )
                ));

            // Generate projection trom T to runtimeType and cast as IQueryable<object>
            IQueryable<object> runtimeTypeSelectExpressionQuery
                = ExpressionMapper.GenerateProjectedQuery<object>(
                    sourceType,
                    runtimeType,
                    bindingsToRuntimeType,
                    source,
                    sourceParameter
            );

            FilteredExpressionQuery<T> resultQuery = new FilteredExpressionQuery<T>();
            resultQuery.PureQuery = runtimeTypeSelectExpressionQuery;
            resultQuery.RuntimeType = runtimeType;
            resultQuery.RuntimeTypeFields = runtimeTypeFields;
            resultQuery.SourceProperties = sourceProperties;
            return resultQuery;
        }

        private static IDictionary<string, PropertyInfo> GetTypeProperties<T>(IEnumerable<string> selectedProperties) where T : class
        {
            var existedProperties = typeof(T)
                .GetProperties()
                .ToDictionary(p => p.Name.ToLowerInvariant());

            IEnumerable<string> properties = selectedProperties as string[] ?? selectedProperties.ToArray();

            if (properties.Any())
            {
                return properties
                   .Where(p => existedProperties.ContainsKey(p.ToLowerInvariant()))
                   .ToDictionary(p => p, p => existedProperties[p.ToLowerInvariant()]);
            }

            return existedProperties;
        }
    }
}