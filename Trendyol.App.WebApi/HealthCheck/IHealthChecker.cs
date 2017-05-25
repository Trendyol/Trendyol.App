namespace Trendyol.App.WebApi.HealthCheck
{
    public interface IHealthChecker
    {
        string Key { get; }

        bool IsCritical { get; }

        bool CheckHealth();
    }
}