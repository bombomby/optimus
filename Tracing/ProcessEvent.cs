using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimus.Tracing
{
    public class ProcessEvent
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String ProcessName { get; set; }
        public String CommandLine { get; set; }
        public String Image { get; set; }
        public int ProcessID { get; set; }
        public ulong UniqueID { get; set; }
        public int ExitStatus { get; set; }

        public bool IsRunning => End < Start;
    }
}
