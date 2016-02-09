using System;
using System.Diagnostics;

namespace PMonitor.Core
{
    /// <summary>
    /// The most common used properties for a process are stored in this class. We can then use it to
    /// display or query instances of this class in an UI applicaiton
    /// </summary>
    public class BasicProcessInformation
    {
        public int Pid { get; set; }
        public string ProcessName { get; set; }
        public string FriendlyName { get; set; }
        public DateTime StartDateTime { get; set; }
        public int MemoryUsageMb { get; set; }
        public ProcessState State { get; set; }
        public int UptimeInSeconds { get; set; }

        public BasicProcessInformation()
        {
            State = ProcessState.NotRunning;
        }

        public BasicProcessInformation(Process winProcess, string friendlyName = null)
        {
            if (winProcess == null)
            {
                return;
            }
            //we have the instance of the process
            ProcessName = winProcess.ProcessName;
            FriendlyName = friendlyName ?? ProcessName;
            StartDateTime = winProcess.StartTime;
            MemoryUsageMb = (int) (winProcess.WorkingSet64/1024/1024);
            State = ProcessState.Running;
            UptimeInSeconds = (int) (DateTime.Now - StartDateTime).TotalSeconds;
        }
    }
}
