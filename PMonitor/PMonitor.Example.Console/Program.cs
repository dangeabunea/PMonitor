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
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            System.Console.WriteLine("PMonitor Example - Console");
            AbstractProcessMonitor pm = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();
            while (true)
            {
                pm.RefreshInformation();
                BasicProcessInformation bpi = pm.GetProcessInformation().Single();

                System.Console.WriteLine(String.Format("{0} Process {1} is {2}", DateTime.Now.ToString(CultureInfo.InvariantCulture), bpi.FriendlyName, bpi.State.ToString()));

                Thread.Sleep(3000);
            }
        }
    }
}
