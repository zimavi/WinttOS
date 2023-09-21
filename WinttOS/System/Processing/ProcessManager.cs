using Cosmos.System.Coroutines;
using System.Collections.Generic;
using WinttOS;

namespace WinttOS.System.Processing
{
    public class ProcessManager
    {
        private List<Process> processes = new();

        public uint ProcessesCount => (uint) processes.Count;

        public bool RegisterProcess(Process process)
        {
            foreach (var _process in processes)
            {
                if (_process == process) return false;
            }
            processes.Add(process);
            processes[processes.Count - 1].SetProcessID((uint)processes.Count - 1);
            processes[processes.Count - 1].Initialize();
            return true;
        }
        public bool RegisterProcess(Process process, ref uint newProcessID)
        {
            foreach (var _process in processes)
            {
                if (_process == process) return false;
            }
            processes.Add(process);
            processes[processes.Count - 1].SetProcessID((uint)processes.Count - 1);
            processes[processes.Count - 1].Initialize();
            newProcessID = (uint)processes.Count - 1;
            return true;
        }

        public bool StartProcess(string processName)
        {
            foreach (var process in processes)
            {
                if (process.ProcessName.Equals(processName)) 
                {
                    if (process.IsProcessRunning)
                        return false;
                    process.Start(); 
                    return true; 
                }
            }
            return false;
        }

        public bool StopProcess(string processName)
        {
            foreach (var process in processes)
            {
                if(process.ProcessName == processName)
                {
                    if (!process.IsProcessRunning)
                        return false;
                    process.Stop();
                    return true;
                }    
            }
            return false;
        }

        public bool StartProcess(uint processID)
        {
            foreach (var process in processes)
            {
                if (process.ProcessID == processID)
                    return StartProcess(process.ProcessName);
            }
            return false;
        }
        public bool StopProcess(uint processID)
        {
            foreach (var process in processes)
            {
                 if(process.ProcessID == processID)
                    return StopProcess(process.ProcessName);
            }
            return false;
        }

        public Process? GetProcessInstance(uint processID)
        {
            foreach(var process in processes)
            {
                if(process.ProcessID == processID)
                    return process;
            }    

            return null;
        }

        public IEnumerator<CoroutineControlPoint> UpdateProcesses()
        {
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (var process in processes)
                    {
                        if (process.Type == (Process.ProcessType) i && process.IsProcessRunning)
                            process.Update();
                    }
                }
                yield return null;

                if (Kernel.IsFinishingKernel)
                {
                    foreach (var process in processes)
                    {
                        if (process.IsProcessRunning)
                            process.Stop();
                    }
                    break;
                }
            }
        }

        public string GetProcessesList()
        {
            string result = "Status\tName\tProcess ID\n";

            foreach (var process in processes)
            {
                if (process.IsProcessRunning)
                    result += $" [X]\t{process.ProcessName}\t{process.ProcessID}\n";
                else
                    result += $" [ ]\t{process.ProcessName}\t{process.ProcessID}\n";
            }

            return result;
        }
    }
}
