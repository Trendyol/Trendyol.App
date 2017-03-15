using System.Collections.Generic;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Requests;

namespace Trendyol.App.EntityFramework.DynamicFiltering
{
    public interface IFilteredExpressionQuery<TResult> where TResult : class
    {
        List<TResult> ToList();

        IPage<TResult> ToPage(PagedRequest request);

        TResult First();

        TResult FirstOrDefault();
    }
}