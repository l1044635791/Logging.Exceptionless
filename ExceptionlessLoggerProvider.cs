using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Exceptionless {
    [ProviderAlias ("Exceptionless")]
    public class ExceptionlessLoggerProvider : ILoggerProvider {
        private readonly ExceptionlessLoggerSender _loggerSender;
        private readonly ExceptionlessLoggerOptions _options;

        public ExceptionlessLoggerProvider (ExceptionlessLoggerOptions options) {
            _options = options;
            _loggerSender = new ExceptionlessLoggerSender (options);
        }

        public ILogger CreateLogger (string name) {
            return new ExceptionlessLogger (name, _loggerSender);
        }

        public void Dispose () {
            _loggerSender.CancellationToken.Cancel ();
            while (!_loggerSender.LoggerSenderDone) {
                Task.Delay (100).Wait ();
            }
        }
    }
}