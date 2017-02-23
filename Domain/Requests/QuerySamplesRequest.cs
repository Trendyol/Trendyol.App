using Trendyol.App.Domain.Requests;

namespace Domain.Requests
{
    public class QuerySamplesRequest : PagedRequest
    {
        public string Fields { get; set; }

        public string Name { get; set; }
    }
}