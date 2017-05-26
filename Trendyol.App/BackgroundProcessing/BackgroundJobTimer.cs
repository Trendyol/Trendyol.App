using System;
using System.Threading;
using Common.Logging;

namespace Trendyol.App.BackgroundProcessing
{
    public class BackgroundJobTimer : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger<BackgroundJobTimer>();
        private readonly int _intervalInMs;
        internal readonly Type JobType;

        private Timer _timer;
        private bool _isRunning;

        public BackgroundJobTimer(Type jobType, int intervalInMs)
        {
            JobType = jobType;
            _intervalInMs = intervalInMs;
        }

        public void InitTimer()
        {
            Logger.Debug($"BackgroundJobManager: Initializing timer for job:{JobType.FullName} with interval:{_intervalInMs}ms.");
            if (_timer == null)
            {
                _timer = new Timer(TimerHandler, null, _intervalInMs, _intervalInMs);
                Logger.Debug($"BackgroundJobManager: Initialized timer for job:{JobType.FullName}.");
            }
        }

        public void Dispose()
        {
            Logger.Debug($"BackgroundJobManager: Disposing timer for job:{JobType.FullName}.");
            if (_timer != null)
            {
                lock (this)
                {
                    _timer.Dispose();
                    _timer = null;

                    while (_isRunning)
                    {
                        Logger.Debug($"BackgroundJobManager: Waiting current operation for job:{JobType.FullName} to be finished before disposing.");
                        Thread.Sleep(100);
                    }

                    Logger.Debug($"BackgroundJobManager: Job:{JobType.FullName} finished and disposed.");
                }
            }
        }

        #region Helpers

        private void TimerHandler(object state)
        {
            _timer.Change(-1, -1);

            Run();

            if (_timer != null)
            {
                _timer.Change(_intervalInMs, _intervalInMs);
            }
        }

        private void Run()
        {
            Logger.Debug($"BackgroundJobManager: Timer hit for job:{JobType.FullName}.");
            if (_isRunning)
            {
                Logger.Debug($"BackgroundJobManager: Timer hit for job:{JobType.FullName} but previous instance is still running. Iteration is cancelled and timer is reset.");
                return;
            }

            _isRunning = true;

            try
            {
                Logger.Debug($"BackgroundJobManager: Trying to execute job:{JobType.FullName}.");
                Execute();
                Logger.Debug($"BackgroundJobManager: Execution of job:{JobType.FullName} successfull.");
            }
            catch (Exception ex)
            {
                Logger.Error($"BackgroundJobManager: Error occured while activating job:{JobType.FullName}.", ex);
            }
            finally
            {
                _isRunning = false;
            }
        }

        private void Execute()
        {
            Thread thread = new Thread(RunTaskInNewThread);
            thread.IsBackground = true;
            thread.Start();
            thread.Join();
        }

        private void RunTaskInNewThread()
        {
            try
            {
                Logger.Debug($"BackgroundJobManager: Trying to create a new instance of job:{JobType.FullName}.");
                IJob job = (IJob)BackgroundJobManager.JobActivator.CreateJobInstance(JobType);

                if (job != null)
                {
                    Logger.Debug($"BackgroundJobManager: Running job:{JobType.FullName}.");
                    job.Run();
                    Logger.Debug($"BackgroundJobManager: Job:{JobType.FullName} run.");
                }
                else
                {
                    Logger.Debug($"BackgroundJobManager: Job:{JobType.FullName} cannot be initialized.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"BackgroundJobManager: Error occured while running job:{JobType.FullName}.", ex);
            }

            GC.Collect();
        }

        #endregion
    }
}