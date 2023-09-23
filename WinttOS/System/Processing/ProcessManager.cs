using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using WinttOS;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.wosh.Utils;

namespace WinttOS.System.Processing
{
    public class ProcessManager
    {
        private List<Process> processes = new();

        public uint ProcessesCount => (uint) processes.Count;

        public bool RegisterProcess(Process process)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.RegisterProcess()",
                "bool(Process)", "ProcessManager.cs", 16));
            foreach (var _process in processes)
            {
                if (_process == process)
                {
                    WinttCallStack.RegisterReturn();
                    return false;
                }
            }
            processes.Add(process);
            processes[processes.Count - 1].SetProcessID((uint)processes.Count - 1);
            processes[processes.Count - 1].Initialize();
            WinttCallStack.RegisterReturn();
            return true;
        }
        public bool RegisterProcess(Process process, ref uint newProcessID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.RegisterProcess()",
                "bool(Process, ref uint)", "ProcessManager.cs", 34));
            foreach (var _process in processes)
            {
                if (_process == process)
                {
                    WinttCallStack.RegisterReturn();
                    return false;
                }
            }
            processes.Add(process);
            processes[processes.Count - 1].SetProcessID((uint)processes.Count - 1);
            processes[processes.Count - 1].Initialize();
            newProcessID = (uint)processes.Count - 1;
            WinttCallStack.RegisterReturn();
            return true;
        }

        public bool StartProcess(string processName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.StartProcess()",
                "bool(string)", "ProcessManager.cs", 54));
            foreach (var process in processes)
            {
                if (process.ProcessName.Equals(processName)) 
                {
                    if (process.IsProcessRunning)
                    {
                        WinttCallStack.RegisterReturn();
                        return false;
                    }
                    process.Start();
                    WinttCallStack.RegisterReturn();
                    return true; 
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        public bool StopProcess(string processName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.StopProcess()",
                "bool(Process)", "ProcessManager.cs", 76));
            foreach (var process in processes)
            {
                if(process.ProcessName == processName)
                {
                    if (!process.IsProcessRunning)
                    {
                        WinttCallStack.RegisterReturn();
                        return false;
                    }
                    process.Stop();
                    WinttCallStack.RegisterReturn();
                    return true;
                }    
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        public bool StartProcess(uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.StartProcess()",
                "bool(uint)", "ProcessManager.cs", 98));
            foreach (var process in processes)
            {
                if (process.ProcessID == processID)
                {
                    WinttCallStack.RegisterReturn();
                    return StartProcess(process.ProcessName);
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }
        public bool StopProcess(uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.StopProcess()",
                "bool(uint)", "ProcessManager.cs", 113));
            foreach (var process in processes)
            {
                if (process.ProcessID == processID)
                {
                    bool isStopped = StopProcess(process.ProcessName);
                    WinttCallStack.RegisterReturn();
                    return isStopped;
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        public Process? GetProcessInstance(uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.GetProcessInstance()",
                "Process?(uint)", "ProcessManager.cs", 130));
            foreach (var process in processes)
            {
                if (process.ProcessID == processID)
                {
                    WinttCallStack.RegisterReturn();
                    return process;
                }
            }
            WinttCallStack.RegisterReturn();
            return null;
        }

        public IEnumerator<CoroutineControlPoint> UpdateProcesses()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.UpdateProcesses()",
                "IEnumerator<CoroutineControlPoint>()", "ProcessManager.cs", 146));
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
                WinttCallStack.RegisterReturn();

                yield return null;

                WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.UpdateProcesses()",
                "IEnumerator<CoroutineControlPoint>()", "ProcessManager.cs", 162));

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
            WinttCallStack.RegisterReturn();
        }

        public void WriteLineProcessesList()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.ProcessManager.WriteLineProcessesList()",
                "void()", "ProcessManager.cs", 180));
            ConsoleColumnFormatter formatter = new(20, 3);
            formatter.Write("Status");
            formatter.Write("Process Name");
            formatter.Write("PID");

            foreach (var process in processes)
            {
                formatter.Write(process.IsProcessRunning ? "[X]" : "[ ]");
                formatter.Write(process.ProcessName);
                formatter.Write(process.ProcessID.ToString());
            }
            WinttCallStack.RegisterReturn();
        }
    }
}
