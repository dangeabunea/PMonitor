using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PMonitor.Core
{
    public abstract class AbstractProcessMonitor
    {
        protected ConcurrentBag<BasicProcessInformation> BasicProcessList { get; }

        public int NbOfProcesses
        {
            get { return BasicProcessList.Count; }
        }

        public int NbOfRunningProcesses
        {
            get { return BasicProcessList.Count(x => x.State == ProcessState.Running); }
        }

        public int NbOfNotRunningProcesses
        {
            get { return BasicProcessList.Count(x => x.State == ProcessState.NotRunning); }
        }

        protected AbstractProcessMonitor()
        {
            BasicProcessList = new ConcurrentBag<BasicProcessInformation>();
        }

        public IList<BasicProcessInformation> GetProcessInformation()
        {
            return new List<BasicProcessInformation>(BasicProcessList);
        }

        /// <summary>
        /// Ovveride this in order to read the names of the processes that you want to monitor. You can read them
        /// from any source, as long as you inititialize the BasicProcessList member
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationException">If there are errors while performing this operation</exception>
        protected abstract void ReadProcessNamesThatNeedToBeMonitored();

        /// <summary>
        /// Ovveride this in order to refresh the information regarding the processes that you are monitoring.
        /// A common use case would be to call this method at a given interval in order to update the status 
        /// of the processes. 
        /// NOTE: The BasicProcessList needs to be initialized first.
        /// 
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If the list of processes to be monitored has not been initialized (equal to 0)</exception>
        protected abstract void RefreshInformation();
    }
}
