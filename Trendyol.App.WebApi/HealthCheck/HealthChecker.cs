using System;

namespace Trendyol.App.WebApi.HealthCheck
{
    internal class HealthChecker
    {
        public string Name { get; set; }

        public Type HealthCheckerType { get; set; }

        public bool IsCritical { get; set; }
    }
}