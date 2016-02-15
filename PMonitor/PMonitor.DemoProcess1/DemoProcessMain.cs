using System;
using System.Threading;

namespace PMonitor.DemoProcess1
{
    /// <summary>
    /// A demo process that runs for 5 seconds before shutting down. It will be sued for testing the process monitor
    /// </summary>
    class DemoProcessMain
    {
        static void Main(string[] args)
        {
            Console.Title = "DemoProcess1";
            Console.WriteLine("Started demo process 1");
            Thread.Sleep(3 * 1000);
            Console.WriteLine("Stopped demo process 1");
        }
    }
}
