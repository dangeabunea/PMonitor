using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMonitor.Core
{
    /// <summary>
    /// The default process monitor for Windows based OS. The names of the processes that will be monitored are taken from
    /// the app.config file. By default, the class will search for the PMonitorProcessNames key, in the appConfig section, where the process anmes will be 
    /// separated by commas;
    /// </summary>
    public class WindowsProcessMonitor : AbstractProcessMonitor
    {
        protected override void ReadProcessNamesThatNeedToBeMonitored()
        {
            throw new NotImplementedException();
        }

        protected override void RefreshInformation()
        {
            throw new NotImplementedException();
        }
    }
}
