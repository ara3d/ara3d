using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara3D;
using System.IO;
using ILogger = Ara3D.ILogger;

namespace UnityBridge
{
    public class UnityLogger : ILogger
    {
        public List<LogEvent> Events = new List<LogEvent>();

        public ILogger Log(string message = "", LogLevel level = LogLevel.None, int eventId = 0)
        {

            var e = new LogEvent
            {
                EventId = eventId,
                Index = Events.Count,
                Message = message,
                When = DateTime.Now
            };
            Events.Add(e);

            UnityEngine.Debug.Log(e);

            return this;
        }

        public void ExportLog(string path)
        {
            File.WriteAllLines(path, Events.Select(e => e.ToString()));
        }
    }
}
