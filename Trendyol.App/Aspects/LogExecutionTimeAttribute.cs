using System;
using System.Diagnostics;
using Common.Logging;
using PostSharp.Aspects;

namespace Trendyol.App.Aspects
{
    [Serializable]
    [DebuggerStepThrough]
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
            args.Proceed();
            stopwatch.Stop();
            long timeSpent = stopwatch.ElapsedMilliseconds;

            if (timeSpent > Threshold)
            {
                Logger.Debug($"Method [{args.Method?.DeclaringType?.Name}.{args.Method?.Name}] took [{timeSpent}] milliseconds to execute.");
            }
        }
    }
}