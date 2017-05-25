using System;
using System.Collections.Generic;

namespace Trendyol.App.WebApi.HealthCheck
{
    internal class HealthCheckerContainer
    {
        internal List<HealthChecker> HealthCheckers;

        public HealthCheckerContainer()
        {
            HealthCheckers = new List<HealthChecker>();
        }

        public void AddHealthChecker(string key, Type healthCheckerType, bool isCritical)
        {
            HealthChecker checker = new HealthChecker
            {
                Name = key,
                HealthCheckerType = healthCheckerType,
                IsCritical = isCritical
            };

            HealthCheckers.Add(checker);
        }
    }
}