using System;
using PMonitor.Core.Linux;
using PMonitor.Core.Windows;

namespace PMonitor.Core
{
    /// <summary>
    /// A factory that will instantiate the correct OS specific implementation of the 
    /// process factory. By default, the factory will return the built in default 
    /// implementaitons
    /// </summary>
    public class ProcessMonitorFactory
    {
        public AbstractProcessMonitor BuildDefaultOSProcessMonitor()
        {
            //we check the OS platform to decide which instance to return
            string osPlatform = Environment.OSVersion.Platform.ToString();
            if (osPlatform.ToLower().Contains("win"))
            {
                return new WindowsProcessMonitor();
            }
            return new LinuxProcessMonitor();
        }
    }
}
