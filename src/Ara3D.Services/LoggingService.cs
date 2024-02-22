using System;
using System.Diagnostics;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public class LogEntry
    {
        public LogEntry(string text, string category, LogLevel level, DateTimeOffset created)
            => (Text, Level, Created) = (text, level, created);
        public LogEntry() {}
        public readonly string Text;
        public readonly LogLevel Level;
        public readonly DateTimeOffset Created;
    }

    /// <summary>
    /// This serves as an example of a service, and
    /// is such a common service that we just built it in. 
    /// </summary>
    public class LoggingService 
        : AggregateModelBackedService<LogEntry>, ILogger
    {
        public Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

        public LoggingService(string category, IApplication app)
            : base(app)
        {
            Category = category;
        }

        public ILogger Log(string message, LogLevel level = LogLevel.None)
        {
            Repository.Add(new LogEntry(message, Category, level, DateTime.Now));
            Debug.WriteLine(Stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff") + " - " + message);
            return this;
        }

        public string Category { get; }
    }
}
