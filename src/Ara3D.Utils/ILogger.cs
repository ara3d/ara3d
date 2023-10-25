using System;
using System.Diagnostics;

namespace Ara3D.Utils
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5,
        Profiling = 6,
    }

    public interface ILogger
    {
        ILogger Log(string message, LogLevel level);
        string Category { get; }
    }

    public static class LoggerExtensions
    {
        public static ILogger Log(this ILogger logger, string message)
            => logger.Log(message, LogLevel.Info);

        public static ILogger LogWarning(this ILogger logger, string message)
            => logger.Log(message, LogLevel.Warning);

        public static ILogger LogDebug(this ILogger logger, string message)
            => logger.Log(message, LogLevel.Debug);

        public static ILogger LogError(this ILogger logger, string message, Exception e)
            => logger.Log($"{e.Message} {message}", LogLevel.Error);
        
        public static ILogger LogError(this ILogger logger, Exception e)
            => logger.LogError("", e);

        public static ILogger LogError(this ILogger logger, string message)
            => logger.Log(message, LogLevel.Error);

        public static Disposer LogDuration(this ILogger logger, string message)
        {
            logger.Log("STARTED: " + message, LogLevel.Profiling);
            var sw = Stopwatch.StartNew();
            return new Disposer(() => logger.Log($"COMPLETED in {sw.ElapsedMilliseconds} msec", LogLevel.Profiling));
        }

        public static Logger SetWriter(this ILogger logger, ILogWriter writer = null)
            => new Logger(writer, logger.Category);
        
        public static Logger Create(this ILogger logger, string category, ILogWriter writer = null)
            => new Logger(writer ?? new LogWriter(), category);
    }
}
