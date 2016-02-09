using System.Linq;

namespace PMonitor.Core.Linux
{
    /// <summary>
    /// Default process monitor for Linux environment. Treats the special case of Mono
    /// processes that have the same name.
    /// </summary>
    public class LinuxProcessMonitor : AbstractProcessMonitor
    {
        public override void RefreshInformation()
        {
            foreach (var basicProcessInformation in BasicProcessList)
            {
                LinuxProcess p = LinuxProcess.GetProcesses().SingleOrDefault(x => x.ProcessName == basicProcessInformation.ProcessName);
                basicProcessInformation.Refresh(p);
            }
        }
    }
}
