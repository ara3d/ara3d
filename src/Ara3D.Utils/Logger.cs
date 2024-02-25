using System;
using System.Diagnostics;

namespace Ara3D.Utils
{
    public class LogEntry
    {
        public readonly DateTimeOffset DateTime = DateTimeOffset.Now;
        public readonly string Category = "";
        public readonly string Message = "";
        public readonly LogLevel Level = LogLevel.Info;

        public LogEntry() 
        { }

        public LogEntry(string message, string category = "", LogLevel level = LogLevel.Info)
        {
            Message = message;
            Category = category;
            Level = level;
        }
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
            Writer.Write(new LogEntry(message, Category, level));
            return this;
        }
    }
}