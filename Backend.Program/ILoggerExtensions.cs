
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Backend.Program
{
    public static class ILoggerExtensions
    {

        public static void Time<T>(this ILogger<T> logger, Action action) where T : class 
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var sw = new Stopwatch();
            
            sw.Start();
            logger.LogInformation($"*** Start ThreadId:{threadId}");

            action();

            sw.Stop();
            logger.LogInformation($"*** End ThreadId:{threadId} in {sw.ElapsedMilliseconds} ms.");
        }
    }
}
