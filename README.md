# PMonitor

PMonitor is a .NET process monitoring library focused on simplicity and ease of use. 

## When would I use this library?

Do you need to know the status of various processes running on your machine? Do you need to display the status of various processes to the user in a console or web application? Do you need to notify users when some critical process is not running? If you answer yes to any of the questions above, then this library is for you. 

## What are the highlights?
-	Compatibility with Linux. You can deploy the application on a Linux machine where it will run under the Mono framework.
-	Ease of use. You do not need to write more than a couple of lines of code.
-	Flexibility. You can change the default behavior of the built in monitors.

## How can I install the library?

Very easy:) You can install this package from Nuget. See the link bellow.

https://www.nuget.org/packages/PMonitor/

## Can you show me some code examples?

### 1. A simple console application 

The application will monitor the status of Notepad process and display it to the console. It will refresh the information every 3 seconds.

The Main class

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
            IProcessMonitor pm = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();

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

But how does the application know to monitor the Notepad process? From the configuration file. That is the place where we need to define the processes that we want to monitor.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- Processes to Monitor -->
    <add key="PMonitorNbOfProcesses" value="1"/>
    
    <!-- 
    The names and friendly names of the processes you want to monitor separated by comma. You can choose the process name 
    from the windows task manager. In this example, I will chose notepad. In windows, the process name does not contain the .exe          extension.
    - notepad is the process name
    - Notepad is the friendly name
    -->
    <add key="PMonitorProcess1" value="notepad,Notepad"/>
  </appSettings>
</configuration>
```

Please note the conventions that are in place.
- We use PMonitorNbOfProcesses key to tell the library how many process we want to monitor
- We use many PMonitorProcessX keys to define the names and friendly names for our proceses. X is the number of the process (1,2,3, etc.) 
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

### 2. A simple web application 

We shall build the application using ASPNET MVC and AngularJS. It will monitor 3 processes: Notepad, Wordpad and Paint. Every five seconds, the application will refresh the HTML page with the current state of the processes.

The MVC Controller

```c#
using System.Collections.Generic;
using System.Web.Mvc;
using PMonitor.Core;

namespace PMonitor.Example.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProcessStatus()
        {
            //Instantiate and refresh information regarding the 3 processes (Notepad, Wordpad and Paint). The
            //information is taken from the Web.config file
            IProcessMonitor processMonitor = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();

            IList<BasicProcessInformation> statusOfProcesses = processMonitor.GetProcessInformation();

            return Json(statusOfProcesses, JsonRequestBehavior.AllowGet);
        }
    }
}
```

The MVC View

```html
@model dynamic

@{
    ViewBag.Title = "PMonitor Example";
}

<html ng-app="app">
<head>
    <!-- We use AngularJS and Bootstrap as external dependencies -->
    <link href="~/Content/bootstrap-3.3.6-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="~/Scripts/angular.min.js"></script>

    <!-- We shall write the view logic in an Angular controller -->
    <script src="~/Scripts/appModule.js"></script>
    <script src="~/Scripts/homeController.js"></script>
</head>
<body ng-controller="HomeController as vm">
    <div class="container">
        <h2>Status of monitored processes</h2>
        <div class="starter-template">
            <p class="lead">Refreshing in {{vm.remainingTimeBeforeRefresh}}</p>
            <br />
            <!-- The table will display the process information -->
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Process Id</th>
                        <th>Name</th>
                        <th>Friendly Name</th>
                        <th>Uptime (sec)</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody ng-repeat="process in vm.monitoredProcesses">
                    <tr ng-class="vm.rowClass(process)">
                        <td>{{process.Pid}}</td>
                        <td>{{process.ProcessName}}</td>
                        <td>{{process.FriendlyName}}</td>
                        <td>{{process.UptimeInSeconds}} sec</td>
                        <td>{{process.State === 0 ? "Running" : "Not Runing"}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
```

The Web.config

```xml
...
    <!-- Processes to Monitor -->
    <add key="PMonitorNbOfProcesses" value="3"/>

    <!-- 
    The names and friendly names of the processes you want to monitor separated by comma. You can choose the process name 
    from the windows task manager. In this example, I will chose notepad, wordpad and paint. In windows, the process names do not
    contain .exe extension.
    -->
    <add key="PMonitorProcess1" value="notepad,Notepad"/>
    <add key="PMonitorProcess2" value="wordpad,Wordpad"/>
    <add key="PMonitorProcess3" value="mspaint,Paint"/>
...
```

The Angular module

```javascript
(function () {
    'use strict';

    angular.module('app', [        
    ]);
})();
```

The Angular controller

```javascript
(function () {
    'use strict';

    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$http','$interval'];

    function HomeController($http, $interval) {
        var vm = this;
        var REFRESH_TIME = 5;

        vm.remainingTimeBeforeRefresh = REFRESH_TIME;

        //We store the monitored processes in this array
        vm.monitoredProcesses = [];

        vm.rowClass = function (process)
        {
            if (process.State === 0) {
                return "success";
            }
            return "warning";
        }

        refresh();

        //Make a request to the server and retrieve the current status of the processes
        function refresh() {
            $interval(function () {
                //each second, we update the remaining time before the call to the server
                vm.remainingTimeBeforeRefresh -= 1;

                //if total time elapsed, we make request to server for status of monitored processes
                //and we re-initialize the remaining time
                if (vm.remainingTimeBeforeRefresh === 0) {
                    $http.get('/Home/GetProcessStatus').then(onRefreshSuccess, onRefreshError);
                }
            }, 1000);
        }

        function onRefreshSuccess(response) {
            vm.remainingTimeBeforeRefresh = REFRESH_TIME;
            vm.monitoredProcesses = response.data;
        }

        function onRefreshError() {
            vm.remainingTimeBeforeRefresh = REFRESH_TIME;
            alert("Error retrieving data.");
        }
    }
})();
```
### 3. I want to read the names of the processes I want to monitor from a different source than the configuration file

If you do not fancy the idea of reading the names of the processes from the application configuration file, then you can change this behavior. And it will only take you five minutes. All you need to do is create a new class that will inherit from WindowsProcessMonitor or LinuxProcessMonitor and override the ReadProcessNamesThatNeedToBeMonitored() method. 

For example, let us say that we want to input those names programmatically, and not depend on any file. We are only interested in the Notepad process.

The custom process monitor implementation 

```c#
using PMonitor.Core;
using PMonitor.Core.Windows;

namespace PMonitor.Example.CustomImpl
{
    //This class will get the name and firendly name of the Notepad process 
    //programatically. There is no need to have an App.config file like in the 
    //standard implementation. Of course, you can read the monitored processes
    //however you like.
    public class NoFileConfigProcessMonitor : WindowsProcessMonitor
    {
        protected override void ReadProcessNamesThatNeedToBeMonitored()
        {
            BasicProcessList.Add(new BasicProcessInformation("notepad","Notepad"));
        }
    }
}
```

The Main class

```c#
using System.Threading;
using PMonitor.Core;

namespace PMonitor.Example.CustomImpl
{
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("PMonitor Example - Console with custom process monitor impl");

            //We can not use the factory to create our process monitor, bacause the factory only
            //returns the built in implementaitons. However, we can just new-up our
            //concrete instance.
            IProcessMonitor pm = new NoFileConfigProcessMonitor();
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
```

## Can I download the complete examples?

Sure! Just download the project and check them out. Each example is placed in its own project:
- PMonitor.Example.Console
- PMonitor.Example.Web
- PMonitor.Example.CustomImpl
