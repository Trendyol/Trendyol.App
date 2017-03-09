using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Objects;
using Trendyol.App.Domain.Requests;

namespace Trendyol.App.EntityFramework.DynamicFiltering
{
    public class FilteredExpressionQuery<TResult> : IFilteredExpressionQuery<TResult>
    {
        public IQueryable<object> PureQuery { get; set; }

        public Type RuntimeType { get; set; }

        public FieldInfo[] RuntimeTypeFields { get; set; }

        public IDictionary<string, PropertyInfo> SourceProperties { get; set; }

        public List<TResult> ToList()
        {
            return ToList(PureQuery);
        }

        public IPage<TResult> ToPage(PagedRequest request)
        {
            if (PureQuery == null)
            {
                throw new ArgumentNullException($"You cannot pageinate on a null object reference. The parameter source should be initialized.", nameof(PureQuery));
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
                        PureQuery = PureQuery.OrderBy(request.OrderBy);
                    }
                    else
                    {
                        PureQuery = PureQuery.OrderBy(request.OrderBy + " descending");
                    }
                }

                return new Page<TResult>(ToList(), 0, 0, 0);
            }

            if (String.IsNullOrEmpty(request.OrderBy))
            {
                throw new InvalidOperationException($"In order to use paging extensions you need to supply an OrderBy parameter.");
            }

            if (request.Order == OrderType.Asc)
            {
                PureQuery = PureQuery.OrderBy(request.OrderBy);
            }
            else
            {
                PureQuery = PureQuery.OrderBy(request.OrderBy + " descending");
            }

            int skip = (request.Page - 1) * request.PageSize;
            int take = request.PageSize;
            int totalItemCount = PureQuery.Count();

            return new Page<TResult>(ToList(PureQuery.Skip(skip).Take(take)), request.Page, request.PageSize, totalItemCount);
        }

        private List<TResult> ToList(IQueryable<object> query)
        {
            // Get result from database
            List<object> listOfObjects = query.ToList();

            MethodInfo castMethod = typeof(Queryable)
                .GetMethod("Cast", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(RuntimeType);

            // Cast list<objects> to IQueryable<runtimeType>
            IQueryable castedSource = castMethod.Invoke(
                null,
                new Object[] { listOfObjects.AsQueryable() }
            ) as IQueryable;

            // Create instance of runtime type parameter
            ParameterExpression runtimeParameter = Expression.Parameter(RuntimeType, "p");

            IDictionary<string, FieldInfo> dynamicTypeFieldsDict =
                RuntimeTypeFields.ToDictionary(f => f.Name, f => f);

            // Generate bindings from runtime type to source type
            IEnumerable<MemberBinding> bindingsToTargetType = SourceProperties.Values
                .Select(property => Expression.Bind(
                    property,
                    Expression.Field(
                        runtimeParameter,
                        dynamicTypeFieldsDict[property.Name.ToLowerInvariant()]
                    )
                ));

            // Generate projection trom runtimeType to T and cast as IQueryable<object>
            IQueryable<TResult> targetTypeSelectExpressionQuery
                = ExpressionMapper.GenerateProjectedQuery<TResult>(
                    RuntimeType,
                    typeof(TResult),
                    bindingsToTargetType,
                    castedSource,
                    runtimeParameter
            );

            // Return list of T
            return targetTypeSelectExpressionQuery.ToList();
        }
    }
}