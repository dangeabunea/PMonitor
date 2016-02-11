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
