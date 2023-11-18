using System;
using System.Diagnostics;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public class LogEntry
    {
        public LogEntry(string text, int category, DateTimeOffset created)
            => (Text, Category, Created) = (text, category, created);
        public LogEntry() {}
        public readonly string Text;
        public int Category;
        public DateTimeOffset Created;
    }

    public class LogRepo : AggregateRepository<LogEntry>
    {
    }

    public class LoggingService : BaseService, ILogger
    {
        public LogRepo Repo { get; set; }
        public Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

        public LoggingService(string category, IApi api, LogRepo repo)
            : base(api)
        {
            Category = category;
            Repo = repo;
        }

        public ILogger Log(string message, LogLevel level = LogLevel.None)
        {
            Repo.Add(new LogEntry(message, (int)level, DateTime.Now));
            Debug.WriteLine(Stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff") + " - " + message);
            return this;
        }

        public string Category { get; }
    }
}
