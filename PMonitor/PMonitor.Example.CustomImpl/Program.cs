using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
