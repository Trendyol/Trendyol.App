using System.Collections.Generic;
using Trendyol.App.Domain.Objects;
using Trendyol.App.Domain.Responses;

namespace Trendyol.App.WebApi.HealthCheck
{
    public class HealthCheckResponse : BaseResponse
    {
        public List<HealthCheckResult> Results { get; set; }
    }
}