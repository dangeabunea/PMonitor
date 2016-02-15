using System.Collections.Generic;

namespace PMonitor.Core
{
    public interface IProcessMonitor
    {
        int NbOfProcesses { get;}
        int NbOfRunningProcesses { get; }
        int NbOfNotRunningProcesses { get; }
        void RefreshInformation();
        IList<BasicProcessInformation> GetProcessInformation();
    }
}