using System.Diagnostics;
using System.Linq;

namespace PMonitor.Core.Windows
{
    /// <summary>
    /// The default process monitor for Windows based OS. The names of the processes that will be monitored are taken from
    /// the app.config file. The keys that are searched in the app config are:
    /// - PMonitorNbOfProcesses
    /// - PMonitorProcessX where x is the index of the process
    /// </summary>
    public class WindowsProcessMonitor : AbstractProcessMonitor
    {
        public override void RefreshInformation()
        {
            foreach (var basicProcessInformation in BasicProcessList)
            {
                Process p = Process.GetProcesses().SingleOrDefault(x => x.ProcessName == basicProcessInformation.ProcessName);
                basicProcessInformation.Refresh(p);
            }
        }
    }
}
