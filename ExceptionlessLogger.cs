using System;

namespace Microsoft.Extensions.Logging.Exceptionless {
    public partial class ExceptionlessLogger : ILogger {
        private readonly ExceptionlessLoggerSender _loggerSender;
        private readonly Func<string, LogLevel, bool> _filter = ((category, logLevel) => true);
        private readonly string _name;

        public ExceptionlessLogger (string name, ExceptionlessLoggerSender loggerSender) {
            _name = string.IsNullOrEmpty (name) ? nameof (ExceptionlessLogger) : name;
            _loggerSender = loggerSender;
        }

        public IDisposable BeginScope<TState> (TState state) {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled (LogLevel logLevel) {
            return _filter (_name, logLevel);
        }

        public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (!IsEnabled (logLevel)) {
                return;
            }

            if (formatter == null) {
                throw new ArgumentNullException (nameof (formatter));
            }

            string message = formatter (state, exception);

            if (string.IsNullOrEmpty (message)) {
                return;
            }

            string[] tags = {logLevel.ToString() };

            _loggerSender.Sender (logLevel, message, _name, exception, tags);
        }

        private class NoopDisposable : IDisposable {
            public static NoopDisposable Instance = new NoopDisposable ();

            public void Dispose () { }
        }
    }
}