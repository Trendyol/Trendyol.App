using System;
using System.Configuration;

namespace Trendyol.App.WebApi.HealthCheck
{
    public class DefaultHealthCheckerActivator : IHealthCheckerActivator
    {
        public IHealthChecker CreateHealthCheckerInstance(Type type)
        {
            try
            {
                object instance = Activator.CreateInstance(type);
                return (IHealthChecker)instance;
            }
            catch (Exception)
            {
                throw new ConfigurationErrorsException($"There was a problem while creating healthchecker instance for type:{type.FullName}. Check if your type implements IHealthChecker interface.");
            }
        }
    }
}