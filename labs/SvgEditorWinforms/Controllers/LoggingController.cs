namespace SvgDemoWinForms
{
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public class LoggingController
    {
        private readonly List<string> _messages = new();
        
        public IReadOnlyList<string> Messages => _messages;

        public void Log(string msg, LogType logType = LogType.Info)
        {
            var dateTimeStr = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var logTypeStr = logType.ToString().ToUpper();
            _messages.Add($"[{logTypeStr}] {dateTimeStr} {msg}");
            LogUpdated?.Invoke(this, EventArgs.Empty);
        }

        public string LastMessage => _messages.Count > 0 ? _messages[_messages.Count - 1] : string.Empty;

        public event EventHandler? LogUpdated;
    }
}
