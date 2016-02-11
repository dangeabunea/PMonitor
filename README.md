# PMonitor

PMonitor is a .NET process monitoring library focused on simplicity and ease of use. 

### Use cases:

Do you need to know the status of various processes running on your machine? Do you need to display the status of various processes to the user in a console or web application? Do you need to notify users when some critical process is not running? If you answer yes to any of the questions above, then this library is for you. 

### Highlights:
-	Compatibility with Linux. You can deploy the application on a Linux machine where it will run under the Mono framework.
-	Ease of use. You do not need to write more than a couple of lines of code.
-	Flexibility. You can change the default behavior of the built in monitors.

### Show me some code:

<b> 1. First Example. </b>A simple console application that monitors the status of Notepad and shows it to the user.

```c#
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
        static void Main(string[] args)
        {
            System.Console.WriteLine("PMonitor Example - Console");
            const int THREE_SECONDS = 3 * 1000;

            //we build an instance of a process monitor by using the ProcessMonitorFactory
            AbstractProcessMonitor pm = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();

            while (true)
            {
                //We update the status of the process every 3 seconds and display it to the console
                pm.RefreshInformation();
                BasicProcessInformation bpi = pm.GetProcessInformation().Single();

                System.Console.WriteLine("{0} Process {1} is {2}", 
                    DateTime.Now.ToString(CultureInfo.InvariantCulture), 
                    bpi.FriendlyName, 
                    bpi.State.ToString());

                Thread.Sleep(THREE_SECONDS);
            }
        }
    }
}
```
