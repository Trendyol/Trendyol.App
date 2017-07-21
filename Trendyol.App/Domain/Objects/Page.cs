using System.Collections.Generic;
using System.Linq;
using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Domain.Objects
{
    public class Page<T> : IPage<T>
    {
        public Page(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            Items = items.ToList();
            Index = pageIndex;
            Size = pageSize;
            TotalCount = totalItemCount;

            if (Size > 0)
            {
                TotalPages = TotalCount / Size;

                if (TotalCount % Size > 0)
                {
                    TotalPages++;
                }
            }

            HasPreviousPage = Index > 1;
            HasNextPage = Index < TotalPages;
        }

        public List<T> Items { get; }
        public int Index { get; }
        public int Size { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }

        public static IPage<T> Empty => new Page<T>(Enumerable.Empty<T>(), 0, 0, 0);
    }
}
