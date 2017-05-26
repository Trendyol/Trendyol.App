using System;

namespace Trendyol.App.WebApi.HealthCheck
{
    public interface IHealthCheckerActivator
    {
        IHealthChecker CreateHealthCheckerInstance(Type type);
    }
}