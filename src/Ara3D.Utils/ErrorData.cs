using System;

namespace Ara3D.Utils
{
    public class ErrorData
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public string HelpLink { get; set; }
        public string Type { get; set; }
        public bool Caught { get; set; }
        public int HResult { get; set; }
        public string StackTrace { get; set; }

        public ErrorData(Exception e, bool caught = true)
        {
            Exception = e;
            Message = e.Message;
            HelpLink = e.HelpLink ?? "";
            Type = e.GetType().Name;
            Caught = caught;
            HResult = e.HResult;
            StackTrace = e.StackTrace ?? "";
        }
    }
}