using System;
using System.Diagnostics;

namespace Ara3D.Utils
{
    public class LogEntry
    {
        public TimeSpan TimeOffset { get; set; } 
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
        public string Category { get; set; } = "";
        public string Message { get; set; } = "";
        public LogLevel Level { get; set; } = LogLevel.Info;
    }

    public interface ILogWriter 
    {
        void Write(LogEntry logEntry);
    }

    public class LogWriter : ILogWriter
    {
        public DateTimeOffset Started { get; } = DateTimeOffset.Now;
        public TimeSpan CurrentTimeElapsed => DateTimeOffset.Now - Started;

        public void Write(LogEntry logEntry)
        {
            var msg = $"[{CurrentTimeElapsed.ToFixedWidthTimeStamp()}] [{logEntry.Level}] [{logEntry.Category}] {logEntry.Message}";
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
    }

    public class Logger : ILogger
    {
        public string Category { get; }
        public ILogWriter Writer { get; }

        public Logger(ILogWriter writer, string category)
        {
            Writer = writer;
            Category = category ?? "";
        }

        public ILogger Log(string message, LogLevel level)
        {
            Writer.Write(new LogEntry { Category = Category, Level = level, Message = message });
            return this;
        }
    }
}