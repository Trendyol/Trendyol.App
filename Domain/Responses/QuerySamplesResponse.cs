using System.Collections.Generic;
using Domain.Objects;
using Trendyol.App.Domain.Responses;

namespace Domain.Responses
{
    public class QuerySamplesResponse : BaseResponse
    {
        public List<Sample> Samples { get; set; }
    }
}