using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Extensions.Logging;

namespace Optimus.Tracing
{
    public class ETWSession : IDisposable
    {
        public TraceEventSession Session { get; set; }
        
        Task ReaderTask { get; set; }

        public ETWSession()
        {
            Session = new TraceEventSession("Optimus.Collector");
            Session.BufferSizeMB = 256;
        }

        public void Start()
        {
            if (ReaderTask == null)
            {
                ReaderTask = Task.Run(() =>
                {
                    Session.Source.Process();
                });
            }
        }

        public void Stop()
        {
            Session.Stop();

            if (!ReaderTask.IsCompleted)
            {
                ReaderTask.Wait();
            }

            ReaderTask = null;

            Session?.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}