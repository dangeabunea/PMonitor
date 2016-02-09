using System;
using System.Diagnostics;

namespace PMonitor.Test
{
    public class FakeProcess : Process
    {
        public virtual string ProcessName { get; set; }
        public virtual int WorkingSet64 { get; set; }
        public virtual int Id { get; set; }
        public virtual DateTime StartTime { get; set; }
    }
}
