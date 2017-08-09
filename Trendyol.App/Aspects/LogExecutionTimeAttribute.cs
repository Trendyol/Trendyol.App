using System;
using System.Diagnostics;
using Common.Logging;
using PostSharp.Aspects;

namespace Trendyol.App.Aspects
{
    [Serializable]
    [DebuggerStepThrough]
    [LinesOfCodeAvoided(31)]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LogExecutionTimeAttribute : MethodInterceptionAspect
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogExecutionTimeAttribute));

        public LogExecutionTimeAttribute(int threshold = -1)
        {
            Threshold = threshold;
        }

        public int Threshold { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                args.Proceed();
                HandleExecutionTimeMeasurementProcess(args, stopwatch);
            }
            catch (Exception ex)
            {
                HandleExecutionTimeMeasurementProcess(args, stopwatch, ex);
                throw;
            }
        }

        private void HandleExecutionTimeMeasurementProcess(MethodInterceptionArgs args, Stopwatch stopwatch, Exception exception = null)
        {
            stopwatch.Stop();
            long timeSpent = stopwatch.ElapsedMilliseconds;
            string className = args.Method?.DeclaringType?.Name;
            string methodName = args.Method?.Name;

            if (exception != null)
            {
                Logger.Trace($"Method [{className}.{methodName}] finished in [{timeSpent}] milliseconds with an exception [{exception.Message}].");
                return;
            }

            if (timeSpent > Threshold)
            {
                Logger.Trace($"Method [{className}.{methodName}] finished in [{timeSpent}] milliseconds.");
            }
        }
    }
}