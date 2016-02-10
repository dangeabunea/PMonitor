using System;

namespace PMonitor.Core.Linux
{
    /// <summary>
    /// The content of the stat files can be found in /proc/{pid}/stat. The values there are separated
    /// with a space. The first one is the PID. The second one is the fileName of the process.
    /// </summary>
    public class LinuxProcessStatusFile
    {
        public int Pid { get; private set; }
        public string FileName { get; private set; }

        public LinuxProcessStatusFile(string text)
        {
            try
            {
                string[] textPart = text.Split(' ');
                Pid = Convert.ToInt16(textPart[0]);
                FileName = textPart[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not parse line" + ex.Message);
            }
        }
    }
}
