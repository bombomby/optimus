using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimus.Tracing
{
    public class PerformanceTracer
    {
        ETWSession? ETW { get; set; }
        public List<ProcessEvent> ProcessEvents = new List<ProcessEvent>();
        public Dictionary<ulong, ProcessEvent> ActiveProcesses = new Dictionary<ulong, ProcessEvent>();

        public void Start()
        {
            if (ETW != null)
                Stop();

            ETW = new ETWSession();
            ETW.Session.EnableKernelProvider(Microsoft.Diagnostics.Tracing.Parsers.KernelTraceEventParser.Keywords.Process);
            ETW.Session.Source.Kernel.ProcessStart += Kernel_ProcessStart;
            ETW.Session.Source.Kernel.ProcessStop += Kernel_ProcessStop;
            ETW.Start();
        }

        public void Stop()
        {
            ETW?.Dispose();
            ETW = null;
        }

        private void Kernel_ProcessStart(Microsoft.Diagnostics.Tracing.Parsers.Kernel.ProcessTraceData obj)
        {
            ProcessEvent ev = new ProcessEvent()
            {
                Start = obj.TimeStamp,
                CommandLine = obj.CommandLine,
                ProcessName = obj.ProcessName,
                ProcessID = obj.ProcessID,
                UniqueID = obj.UniqueProcessKey,
                Image = obj.ImageFileName,
            };
            ProcessEvents.Add(ev);
            ActiveProcesses.Add(ev.UniqueID, ev);
            OnProcessChanged?.Invoke(ev);
        }

        private void Kernel_ProcessStop(Microsoft.Diagnostics.Tracing.Parsers.Kernel.ProcessTraceData obj)
        {
            ProcessEvent ev = null;
            if (ActiveProcesses.TryGetValue(obj.UniqueProcessKey, out ev))
            {
                ev.End = obj.TimeStamp;
                ev.ExitStatus = obj.ExitStatus;
                ActiveProcesses.Remove(obj.UniqueProcessKey);
                OnProcessChanged?.Invoke(ev);
            }
        }

        public delegate void OnProcessChangedHandler(ProcessEvent ev);
        public event OnProcessChangedHandler OnProcessChanged;
    }
}
