using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Exceptionless {
    public class ExceptionlessLoggerOptions {
        public Func<string, LogLevel, bool> Filter { get; set; } = ((category, logLevel) => true);
        public int MaxQueueData { get; set; }
        public int Delay { get; set; }
    }
    public class LogInfo {
        public LogInfo (string level, string message, string source, Exception ex, string[] tags) {
            Level = level;
            Message = message;
            Source = source;
            Ex = ex;
            Tags = tags;
        }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public Exception Ex { get; set; }
        public string[] Tags { get; set; }
    }
}