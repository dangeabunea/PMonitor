using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using PMonitor.Core;

namespace PMonitor.Example.Console
{
    class Program
    {
        /// <summary>
        /// On Windows, interrogate the status of the Notepad process every 3 seconds. You can open and close
        /// Notepad in order to see how the program responds to your changes. We print the output on the console.
        /// </summary>
        static void Main()
        {
            System.Console.WriteLine("PMonitor Example - Console");
            IProcessMonitor pm = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();
            while (true)
            {
                pm.RefreshInformation();
                BasicProcessInformation bpi = pm.GetProcessInformation().Single();

                System.Console.WriteLine("{0} Process {1} is {2}", DateTime.Now.ToString(CultureInfo.InvariantCulture), bpi.FriendlyName, bpi.State.ToString());

                Thread.Sleep(3000);
            }
        }
    }
}
