# PMonitor

PMonitor is a .NET process monitoring library focused on simplicity and ease of use. 

## Use cases:

Do you need to know the status of various processes running on your machine? Do you need to display the status of various processes to the user in a console or web application? Do you need to notify users when some critical process is not running? If you answer yes to any of the questions above, then this library is for you. 

## Highlights:
-	Compatibility with Linux. You can deploy the application on a Linux machine where it will run under the Mono framework.
-	Ease of use. You do not need to write more than a couple of lines of code.
-	Flexibility. You can change the default behavior of the built in monitors.

## Show me some code:

### 1. A simple console application that monitors the status of Notepad and shows it to the user.

The C# code

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

But how does the applicaiton know to monitor the Notepad process. From the configuration file. That is the place where we need to define the processes that we want to monitor.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- Processes to Monitor -->
    <add key="PMonitorNbOfProcesses" value="1"/>
    
    <!-- 
    The name and friendly name of the process you want to monitor separated by comma. You can Choose the process name 
    from the windows task manager. In this example, I will chose notepad. In windows, the process name is found without
    adding the .exe extension.
    - notepad is the process name
    - Notepad is the friendly name
    -->
    <add key="PMonitorProcess1" value="notepad,Notepad"/>
  </appSettings>
</configuration>
```

Please note the conventions that are in place.
- We use PMonitorNbOfProcesses key to tell the library how many process we want to monitor
- We use many PMonitorProcessX keys to define the names and friendly names for our proceses. X is the number of the process (1,2,3,etc) 
For example, if we want to monitor 2 processes, we can change the configuration file like this:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- Processes to Monitor -->
    <add key="PMonitorNbOfProcesses" value="2"/>
    
    <add key="PMonitorProcess1" value="notepad,Notepad"/>
    <add key="PMonitorProcess2" value="wordpad,Wordpad"/>
  </appSettings>
</configuration>
```

### 2. A simple web application updates and displays a list of processes in a html page.

