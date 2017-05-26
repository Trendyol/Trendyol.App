using System;

namespace Trendyol.App.BackgroundProcessing
{
    public class DefaultJobActivator : IJobActivator
    {
        public T CreateJobInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public object CreateJobInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}