using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PMonitor.Core.Linux
{
    /// <summary>
    /// We need a custom class that will encapsulate a Linux porcess. Under Mono, the Process.GetProcessesByName() will
    /// not work in the same way as on Windows. Each applicaiton that you run under Mono, will have the same process name: mono.
    /// So, we need to differentiate between them.
    /// </summary>
    public class LinuxProcess
    {
        private readonly Process _process;

        public int Id
        {
            get
            {
                if (_process != null)
                {
                    return _process.Id;
                }
                return default(int);
            }
        }
        public string ProcessName { get; private set; }
        public long WorkingSet64
        {
            get
            {
                if (_process != null)
                {
                    return _process.WorkingSet64;
                }
                return default(int);
            }
        }
        public DateTime StartTime
        {
            get
            {
                if (_process != null)
                {
                    return _process.StartTime;
                }
                return default(DateTime);
            }
        }

        private LinuxProcess(int pid, string processName)
        {
            _process = Process.GetProcessById(pid);

            if (_process == null)
            {
                Console.WriteLine("Could not find process by PID " + pid);
            }

            ProcessName = processName;
        }

        private LinuxProcess(Process process)
        {
            _process = process;
        }

        public void Kill()
        {
            _process.Kill();
        }

        //Static Methods. We use them in order to replicate how the Process class functions

        public static LinuxProcess Start(string fileName)
        {
            Process p = Process.Start(fileName);
            return new LinuxProcess(p);
        }

        /// <summary>
        /// Get the Unix Process by searching the /proc files. The /proc directory is a virtual file system,
        /// sometimes reffered to as the process information pseudo file system.It doesn't contain 'real' files but runtime system information.
        /// Each of the numbered directories corresponds to an actual process ID. Details of this process can 
        /// be obtained by looking at the associated files in the directory for this process, for example /proc/460. Inside this folder we can find more directories:
        /// - /proc/PID/cmdline for command line arguments that were used to lanch the process
        /// - /proc/PID/stat for process status
        /// </summary>
        /// <returns></returns>
        public static LinuxProcess[] GetProcesses()
        {
            const string PROC_DIR = "/proc";

            IList<LinuxProcess> processList = new List<LinuxProcess>();

            foreach (var directory in Directory.GetDirectories(PROC_DIR))
            {
                var directoryName = Path.GetFileName(directory);

                //We want to search only through the files that are associated with a PID, that have
                //filenames represented as numbers. So, if we can not convert it to an int, then we skip the 
                //directory
                int pid;
                bool isPidDirectory = int.TryParse(directoryName, out pid);
                if (!isPidDirectory)
                {
                    continue;
                }

                //Parse the process status file in order to get the filename of that lanched the process. If no stat
                //file is present, than we skip and continue with the next directory
                LinuxProcessStatusFile processStatusFile = ParseLinuxProcessStatusFile(pid);
                if (processStatusFile == null)
                {
                    continue;
                }

                //Build Linux process and add it to our result set
                LinuxProcess linuxProcess = BuildProcess(processStatusFile);
                processList.Add(linuxProcess);
            }

            return processList.ToArray();
        }

        private static LinuxProcess BuildProcess(LinuxProcessStatusFile processStatusFile)
        {
            string processName = GetProcessName(processStatusFile);
            LinuxProcess linuxProcess = new LinuxProcess(processStatusFile.Pid, processName);
            return linuxProcess;
        }

        private static string GetProcessName(LinuxProcessStatusFile processStatusFile)
        {
            //We verify if the process is a mono app. They are present in the stat file if the (mono) text is there. If
            //this is the case, then we gho deeper and anlyze the cmdline file in order to get the arguments to the mono command.
            //Those will tell us what was the .exe applicaiton that was executed.
            if (processStatusFile.FileName == "(mono)")
            {
                // We can find the command line text, including arguments for each process. We find it in /proc/{pid}/cmdline. It is here where we
                // can seacrh for the name of the .exe file that was executed under Mono, as it is an argument for the mono command used to execute
                // our .NET code under Mono
                IList<string> cmdLineArgs;
                const string CMDLINE_PATTERN = "/proc/{0}/cmdline";
                using (var fileReader = File.OpenText(string.Format(CMDLINE_PATTERN, processStatusFile.Pid)))
                {
                    string contents = fileReader.ReadToEnd();
                    cmdLineArgs = contents.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
                }

                //the first element is the mono command
                //the second element is the program we executed (.exe)
                return Path.GetFileName(cmdLineArgs.ElementAt(1));
            }
            //normal process, we just trim the paranthesis
            return processStatusFile.FileName.Trim('(', ')');
        }

        public static LinuxProcess[] GetProcessesByName(string processName)
        {
            return GetProcesses().Where(p => p.ProcessName == processName).ToArray();
        }

        public static LinuxProcess GetProcessById(int id)
        {
            return GetProcesses().SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// We can find the stat text for each process. We find it in the /proc/{pid}/stat
        /// </summary>
        /// <param name="pid"></param>
        /// <returns>The parsed result after reading the file</returns>
        private static LinuxProcessStatusFile ParseLinuxProcessStatusFile(int pid)
        {
            const string STAT_PATH_PATTERN = "/proc/{0}/stat";

            try
            {
                var statFilePath = string.Format(STAT_PATH_PATTERN, pid);
                if (!File.Exists(statFilePath))
                {
                    return null;
                }

                using (var fileReader = File.OpenText(statFilePath))
                {
                    try
                    {
                        var line = fileReader.ReadToEnd();
                        return new LinuxProcessStatusFile(line);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("Could not parse line for PID {0} because \"{1}\".", pid, ex.Message));
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
