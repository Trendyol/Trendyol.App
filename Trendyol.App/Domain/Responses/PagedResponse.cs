using System.Collections.Generic;
using Newtonsoft.Json;
using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Domain.Responses
{
    public abstract class PagedResponse<T> : BaseResponse<List<T>>
    {
        private readonly IPage<T> _page;

        protected PagedResponse(IPage<T> page)
        {
            _page = page;
            Data = page.Items;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int PageIndex => _page.Index;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int PageSize => _page.Size;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalCount => _page.TotalCount;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalPages => _page.TotalPages;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool HasPreviousPage => _page.HasPreviousPage;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool HasNextPage => _page.HasNextPage;

        public bool ShouldSerializePageIndex()
        {
            return _page.Index > 0;
        }

        public bool ShouldSerializePageSize()
        {
            return _page.Index > 0;
        }

        public bool ShouldSerializeTotalCount()
        {
            return _page.Index > 0;
        }

        public bool ShouldSerializeTotalPages()
        {
            return _page.Index > 0;
        }

        public bool ShouldSerializeHasPreviousPage()
        {
            return _page.Index > 0;
        }

        public bool ShouldSerializeHasNextPage()
        {
            return _page.Index > 0;
        }
    }
}