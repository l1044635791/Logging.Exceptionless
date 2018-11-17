using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exceptionless;

namespace Microsoft.Extensions.Logging.Exceptionless {
    public class ExceptionlessLoggerSender {
        private ConcurrentQueue<LogInfo> _queue = new ConcurrentQueue<LogInfo> ();

        public ExceptionlessLoggerSender (ExceptionlessLoggerOptions options) {
            Options = options;
            BeginQueueSender ();
        }

        public CancellationTokenSource CancellationToken => new CancellationTokenSource ();

        public bool LoggerSenderDone => _queue.Count == 0;

        public ExceptionlessLoggerOptions Options { get; }

        public void Sender (LogLevel level, string message, string name, Exception exception, string[] tags) {
            _queue.Enqueue (new LogInfo (level.ToString (), message, name, exception, tags));
        }

        private void BeginQueueSender () {
            Task.Run (() => {
                while (!CancellationToken.IsCancellationRequested || _queue.Count > 0) {
                    int currentCount = _queue.Count;
                    if (currentCount == 0) {
                        Thread.Sleep (Options.Delay);
                        continue;
                    }

                    currentCount = currentCount > Options.MaxQueueData ? Options.MaxQueueData : currentCount;

                    for (int i = 0; i < currentCount; i++) {
                        _queue.TryDequeue (out LogInfo log);
                        ExceptionlessClient.Default.CreateLog (log.Source, log.Message, log.Level).AddTags (log.Tags).Submit ();
                    }
                }
            });
        }
    }
}