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
        public int Pid { get; private set; }
        public string ProcessName { get; private set; }
        public string FriendlyName { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public int MemoryUsageMb { get; private set; }
        public ProcessState State { get; private set; }
        public int UptimeInSeconds { get; private set; }

        public BasicProcessInformation()
        {
            State = ProcessState.NotRunning;
        }

        public BasicProcessInformation(string processName) : this()
        {
            ProcessName = processName;
            FriendlyName = processName;
        }

        public BasicProcessInformation(string processName, string friendlyName) : this()
        {
            ProcessName = processName;
            FriendlyName = friendlyName;
        }

        /// <summary>
        /// Refresh the information about the associated process.
        /// </summary>
        /// <param name="process">The associated process where the information is retrieved from</param>
        public void Refresh(Process process)
        {
            //if process can not be found, re-initialize all members of the process information object
            if (process == null)
            {
                Pid = default(int); ;
                StartDateTime = default(DateTime);
                MemoryUsageMb = default(int);
                UptimeInSeconds = default(int);
                State = ProcessState.NotRunning;
                return;
            }
            //else update the information of this object from the associated process
            Pid = process.Id;
            StartDateTime = process.StartTime;
            MemoryUsageMb = (int) (process.WorkingSet64/1024/1024);
            UptimeInSeconds = (int) (DateTime.Now - process.StartTime).TotalSeconds;
            State = ProcessState.Running;
        }
    }
}
