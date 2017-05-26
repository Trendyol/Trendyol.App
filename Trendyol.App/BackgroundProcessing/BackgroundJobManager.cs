using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;

namespace Trendyol.App.BackgroundProcessing
{
    public class BackgroundJobManager
    {
        private static readonly ILog Logger = LogManager.GetLogger<BackgroundJobManager>();
        private static readonly List<BackgroundJobTimer> Timers = new List<BackgroundJobTimer>();
        internal static IJobActivator JobActivator = new DefaultJobActivator();

        public static void Start()
        {
            Logger.Debug("BackgroundJobManager: Start requested.");

            if (Timers != null && Timers.Any())
            {
                Logger.Debug($"BackgroundJobManager: {Timers.Count} job found.");
                foreach (BackgroundJobTimer timer in Timers)
                {
                    Logger.Debug($"BackgroundJobManager: Creating timer for job:{timer.JobType.FullName}.");
                    timer.InitTimer();
                }
            }
        }

        public static void Stop()
        {
            Logger.Debug("BackgroundJobManager: Stop requested.");

            if (Timers != null && Timers.Any())
            {
                foreach (BackgroundJobTimer timer in Timers)
                {
                    Logger.Debug($"BackgroundJobManager: Disposing timer for job:{timer.JobType.FullName}.");
                    timer.Dispose();
                }
            }
        }

        public static void SetJobActivator(IJobActivator jobActivator)
        {
            JobActivator = jobActivator;
            Logger.Debug($"BackgroundJobManager: JobActivator:{jobActivator.GetType().FullName} is registered.");
        }

        public static void Register<T>(int jobIntervalInMs) where T : IJob
        {
            Logger.Debug($"BackgroundJobManager: Registering timer for job:{typeof(T).FullName}.");
            Timers.Add(new BackgroundJobTimer(typeof(T), jobIntervalInMs));
        }
    }
}