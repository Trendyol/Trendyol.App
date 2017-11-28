using Trendyol.App.BackgroundProcessing;

namespace Trendyol.App.Autofac.BackgroundProcessing
{
    public static class BackgroundJobManagerExtensions
    {
        public static void UseAutofacActivator(this BackgroundJobManager backgroundJobManager)
        {
            AutofacActivator autofacActivator = new AutofacActivator();
            backgroundJobManager.SetJobActivator(autofacActivator);
        }
    }
}