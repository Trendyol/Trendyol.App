using System.Collections.Generic;
using System.Threading.Tasks;
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

        Task<List<TResult>> ToListAsync();

        Task<IPage<TResult>> ToPageAsync(PagedRequest request);

        Task<TResult> FirstAsync();

        Task<TResult> FirstOrDefaultAsync();
    }
}