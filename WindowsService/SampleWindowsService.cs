using Trendyol.App.BackgroundProcessing;
using Trendyol.App.Daemon;
using Trendyol.App.Autofac.BackgroundProcessing;

namespace WindowsService
{
    public class SampleWindowsService : TrendyolWindowsService
    {
        private static readonly BackgroundJobManager backgroundJobManager = new BackgroundJobManager();

        public override void Start()
        {
            backgroundJobManager.Register<TestJob>(1000);
            backgroundJobManager.UseAutofacActivator();
            backgroundJobManager.Start();
        }

        public override void Stop()
        {
            backgroundJobManager.Stop();
        }
    }
}