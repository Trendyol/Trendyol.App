using System.Collections.Generic;
using System.Linq;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Requests;

namespace Trendyol.App.EntityFramework.DynamicFiltering
{
    public interface IFilteredExpressionQuery<TResult>
    {
        List<TResult> ToList();

        IPage<TResult> ToPage(PagedRequest request);

        TResult First();

        TResult FirstOrDefault();
    }
}