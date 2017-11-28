using Autofac;
using System;
using Trendyol.App.BackgroundProcessing;

namespace Trendyol.App.Autofac.BackgroundProcessing
{
    public class AutofacActivator : IJobActivator
    {
        public T CreateJobInstance<T>()
        {
            return TrendyolApp.Instance.GetAutofacContainer().Resolve<T>();
        }

        public object CreateJobInstance(Type type)
        {
            return TrendyolApp.Instance.GetAutofacContainer().Resolve(type);
        }
    }
}