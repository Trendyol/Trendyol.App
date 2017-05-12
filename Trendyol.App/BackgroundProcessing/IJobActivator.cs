using System;

namespace Trendyol.App.BackgroundProcessing
{
    public interface IJobActivator
    {
        T CreateJobInstance<T>();

        object CreateJobInstance(Type type);
    }
}