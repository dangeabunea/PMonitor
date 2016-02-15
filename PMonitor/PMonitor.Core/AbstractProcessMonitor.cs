using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PMonitor.Core
{
    /// <summary>
    /// The basic class for all instances of process monitors
    /// </summary>
    public abstract class AbstractProcessMonitor : IProcessMonitor
    {
        //We make the collection concurrent inc ase refreshes and reads are performed from multiple threads
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
            ReadProcessNamesThatNeedToBeMonitored();
            RefreshInformation();
        }

        /// <summary>
        /// Returns an order list (by process name) of all monitored process information members
        /// </summary>
        /// <returns></returns>
        public IList<BasicProcessInformation> GetProcessInformation()
        {
            return new List<BasicProcessInformation>(BasicProcessList).OrderBy(x => x.ProcessName).ToList();
        }

        /// <summary>
        /// Default implementation will search in the App.config. Ovveride this in order to read the names of the processes that 
        /// you want to monitor from a custom source. 
        /// You can read them from any source, as long as you inititialize the BasicProcessList member
        /// </summary>
        /// <exception cref="ApplicationException">If there are errors while performing this operation</exception>
        protected virtual void ReadProcessNamesThatNeedToBeMonitored()
        {
            const string NB_KEY = "PMonitorNbOfProcesses";
            const string PROCESS_BASE_NAME_KEY = "PMonitorProcess";

            try
            {
                int nbProcessesToMonitor = Int32.Parse(ConfigurationManager.AppSettings[NB_KEY]);
                for (int i = 1; i <= nbProcessesToMonitor; i++)
                {
                    string processName =
                        ConfigurationManager.AppSettings[PROCESS_BASE_NAME_KEY + i].Split(',').ElementAt(0);
                    string processFriendlyName =
                        ConfigurationManager.AppSettings[PROCESS_BASE_NAME_KEY + i].Split(',').ElementAt(1);
                    BasicProcessList.Add(new BasicProcessInformation(processName, processFriendlyName));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while reading the process names from the App.config file.", ex);
            }
        }

        /// <summary>
        /// Ovveride this in order to refresh the information regarding the processes that you are monitoring.
        /// A common use case would be to call this method at a given interval in order to update the status 
        /// of the processes. 
        /// NOTE: The BasicProcessList needs to be initialized first.
        /// </summary>
        public abstract void RefreshInformation();
    }
}
