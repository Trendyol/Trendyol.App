using Autofac;

namespace Trendyol.App.Autofac
{
    public static class TrendyolAppExtensions
    {
        public static IContainer GetAutofacContainer(this TrendyolApp app)
        {
            IContainer container = app.DataStore.GetData<IContainer>(Constants.AutofacContainerDataKey);
            return container;
        }
    }
}