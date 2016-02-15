using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PMonitor.Core;

namespace PMonitor.Test
{
    /// <summary>
    /// In order to simulate a process, we shall use the PMonitor.DemoProcess1.exe console
    /// applicaiton. We need to copy it to the /Resources folder, so that it can be executed
    /// from the unit tests. Once we have this, we can simulate a process that we want to monitor.
    /// 
    /// The tests will focus on:
    /// - initialization of the process monitor from the App.config file
    /// - intercaption of running and not-running processes
    /// - long run with process first started and then stopped
    /// </summary>

    [TestFixture]
    public class ProcessMonitorTests
    {
        private readonly string _process1FilePath;
        private readonly string _process2FilePath;

        public ProcessMonitorTests()
        {
            var resourcesPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Resources";
            _process1FilePath = Path.Combine(resourcesPath, "PMonitor.DemoProcess1.exe");
            _process2FilePath = Path.Combine(resourcesPath, "PMonitor.DemoProcess2.exe");
        }

        [Test]
        public void TheConstructorShouldInstantiateMonitorCorrectly()
        {
            //Act
            IProcessMonitor pMonitor = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();

            //Assert
            Assert.AreEqual(2, pMonitor.NbOfProcesses, "NbOfProcesses assertion is wrong");
            Assert.AreEqual("PMonitor.DemoProcess1", pMonitor.GetProcessInformation().First().ProcessName,
                "ProcessName1 assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().First().State,
                "ProcessState1 assertion is wrong");
            Assert.AreEqual("PMonitor.DemoProcess2", pMonitor.GetProcessInformation().Last().ProcessName,
                "ProcessName2 assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().Last().State,
                "ProcessState2 assertion is wrong");
        }

        [Test]
        public void WhenFirstProcessIsStartedShouldUpdateTheProcessStatus()
        {
            //Arrange
            Process demoProcess1 = Process.Start(_process1FilePath);

            //Act
            IProcessMonitor pMonitor = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();
            pMonitor.RefreshInformation();

            //Assert
            Assert.AreEqual(2, pMonitor.NbOfProcesses, "NbOfProcesses assertion is wrong");
            Assert.AreEqual(1, pMonitor.NbOfNotRunningProcesses, "NbOfNotRunningProcesses assertion is wrong");
            Assert.AreEqual(1, pMonitor.NbOfRunningProcesses, "NbOfRunningProcesses assertion is wrong");
            Assert.AreEqual(ProcessState.Running, pMonitor.GetProcessInformation().First().State,
                "ProcessState1 assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().Last().State,
                "ProcessState2 assertion is wrong");

            //Clean
            demoProcess1.Kill();
        }

        [Test]
        public void WhenFirstProcessIsRunningAndThenStoppedShouldUpdateTheProcessStatus()
        {
            //Arrange
            Process demoProcess1 = Process.Start(_process1FilePath);

            //Act
            IProcessMonitor pMonitor = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();
            pMonitor.RefreshInformation();

            //Assert
            Assert.AreEqual(2, pMonitor.NbOfProcesses, "NbOfProcesses assertion is wrong");
            Assert.AreEqual(1, pMonitor.NbOfNotRunningProcesses, "NbOfNotRunningProcesses assertion is wrong");
            Assert.AreEqual(1, pMonitor.NbOfRunningProcesses, "NbOfRunningProcesses assertion is wrong");
            Assert.AreEqual(ProcessState.Running, pMonitor.GetProcessInformation().First().State,
                "ProcessState1 assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().Last().State,
                "ProcessState2 assertion is wrong");

            //Act
            demoProcess1.Kill();
            pMonitor.RefreshInformation();

            //Assert
            Assert.AreEqual(2, pMonitor.NbOfProcesses, "NbOfProcesses assertion is wrong");
            Assert.AreEqual(2, pMonitor.NbOfNotRunningProcesses, "NbOfNotRunningProcesses assertion is wrong");
            Assert.AreEqual(0, pMonitor.NbOfRunningProcesses, "NbOfRunningProcesses assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().First().State,
                "ProcessState1 assertion is wrong");
            Assert.AreEqual(ProcessState.NotRunning, pMonitor.GetProcessInformation().Last().State,
                "ProcessState2 assertion is wrong");
        }
    }
}
