using Trendyol.App.Domain.Objects;

namespace Trendyol.App.WebApi.HealthCheck
{
    public interface IHealthChecker
    {
        HealthCheckResult CheckHealth();
    }
}