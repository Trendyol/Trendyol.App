using Trendyol.App.Domain.Enums;

namespace Trendyol.App.Domain.Requests
{
    public class PagedRequest
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public OrderType Order { get; set; }

        public string OrderBy { get; set; }
    }
}