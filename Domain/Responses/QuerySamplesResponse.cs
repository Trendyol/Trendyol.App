using System.Collections.Generic;
using Domain.Objects;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Responses;

namespace Domain.Responses
{
    public class QuerySamplesResponse : PagedResponse<Sample>
    {
        public QuerySamplesResponse(IPage<Sample> page) 
            : base(page)
        {
        }

        public override List<Sample> Data { get; set; }
    }
}