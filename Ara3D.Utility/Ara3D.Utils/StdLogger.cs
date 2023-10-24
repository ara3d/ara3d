using System;
using System.Diagnostics;

namespace Ara3D.Utils
{
    public class StdLogger : ILogger
    {
        public Stopwatch Stopwatch = Stopwatch.StartNew();

        private readonly bool _writeToConsole;
        private readonly bool _writeToDebug;

        public StdLogger(string category = "", bool writeToConsole = true, bool writeToDebug = true)
        {
            Category = category;
            _writeToConsole = writeToConsole;
            _writeToDebug = writeToDebug;
        }

        public ILogger Log(string message = "", LogLevel level = LogLevel.None)
        {
            var timeStamp = Stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.ff");
            var msg = $"[{timeStamp}][{level:G}] {message}";
            if (_writeToConsole) Console.WriteLine(msg);
            if (_writeToDebug) Debug.WriteLine(msg);
            return this;
        }
        
        public string Category { get; }
    }
}
