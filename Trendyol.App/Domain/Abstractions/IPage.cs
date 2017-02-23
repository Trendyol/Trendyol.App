using System.Collections.Generic;

namespace Trendyol.App.Domain.Abstractions
{
    public interface IPage<T>
    {
        List<T> Items { get; }

        int Index { get; }

        int Size { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }
}