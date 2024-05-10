using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Shell.Utils;

namespace WinttOS.wSystem.Processing
{

    public sealed class ProcessManager
    {
        private List<Process> _processes = new();

        public List<Process> Processes => _processes;

        public uint ProcessesCount => (uint) _processes.Count;

        public ProcessManager()
        {
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "ProcessManager");
        }

        public bool TryRegisterProcess(Process process)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryRegisterProcess()",
                "bool(Process)", "ProcessManager.cs", 16));

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, process.ProcessName);
            ShellUtils.MoveCursorUp();

            foreach (var p in _processes)
            {
                if (p == process)
                {
                    ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.FAILED, process.ProcessName);
                    WinttCallStack.RegisterReturn();
                    return false;
                }
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].CurrentSet = WinttOS.UsersManager.CurrentUser.UserAccess.PrivilegeSet;
            _processes[_processes.Count - 1].Initialize();

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, process.ProcessName);

            WinttCallStack.RegisterReturn();
            return true;
        }
        public bool TryRegisterProcess(Process process, out uint newProcessID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryRegisterProcess()",
                "bool(Process, ref uint)", "ProcessManager.cs", 34));

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, process.ProcessName);
            ShellUtils.MoveCursorUp();

            foreach (var _process in _processes)
            {
                if (_process == process)
                {
                    ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.FAILED, process.ProcessName);
                    WinttCallStack.RegisterReturn();
                    newProcessID = 0;
                    return false;
                }
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].CurrentSet = WinttOS.UsersManager.CurrentUser.UserAccess.PrivilegeSet;
            _processes[_processes.Count - 1].Initialize();
            newProcessID = (uint)_processes.Count - 1;
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, process.ProcessName);
            WinttCallStack.RegisterReturn();
            return true;
        }

        public bool TryStartProcess(string processName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryStartProcess()",
                "bool(string)", "ProcessManager.cs", 54));

            ShellUtils.PrintTaskResult("Starting", ShellTaskResult.DOING, processName);
            ShellUtils.MoveCursorUp();

            foreach (var process in _processes)
            {
                if (process.ProcessName.Equals(processName)) 
                {
                    if (process.IsProcessRunning)
                    {
                        ShellUtils.PrintTaskResult("Starting", ShellTaskResult.FAILED, processName);
                        WinttCallStack.RegisterReturn();
                        return false;
                    }
                    ShellUtils.PrintTaskResult("Starting", ShellTaskResult.OK, processName);
                    process.Start();
                    WinttCallStack.RegisterReturn();
                    return true; 
                }
            }
            ShellUtils.PrintTaskResult("Starting", ShellTaskResult.FAILED, processName);
            WinttCallStack.RegisterReturn();
            return false;
        }

        public bool TryStopProcess(string processName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryStopProcess()",
                "bool(Process)", "ProcessManager.cs", 76));

            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.DOING, processName);
            ShellUtils.MoveCursorUp();

            foreach (var process in _processes)
            {
                if(process.ProcessName == processName)
                {
                    if (!process.IsProcessRunning)
                    {
                        ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, processName);
                        WinttCallStack.RegisterReturn();
                        return false;
                    }
                    process.Stop();
                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.OK, processName);
                    WinttCallStack.RegisterReturn();
                    return true;
                }    
            }
            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, processName);
            WinttCallStack.RegisterReturn();
            return false;
        }

        public void RunProcessGC()
        {
            foreach(var process in _processes)
            {
                if (process.Type.Value <= Process.ProcessType.Driver.Value)
                    continue;
                if (process.HasOwnerProcess && (!process.OwnerProcess.IsProcessRunning || !process.OwnerProcess.IsProcessInitialized))
                {
                    _processes.Remove(process.OwnerProcess);
                }
                if (!process.IsProcessRunning || !process.IsProcessInitialized)
                {
                    _processes.Remove(process);
                }
            }    
        }

        public bool TryRemoveDeadProcess(uint processId)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryRemoveDeadProcess()"));

            foreach(Process p in _processes)
            {
                if(p.ProcessID == processId)
                {
                    if(!p.IsProcessRunning)
                    {
                        _processes.Remove(p);
                        WinttCallStack.RegisterReturn();
                        return true;
                    }

                    break;
                }
            }

            WinttCallStack.RegisterReturn();
            return false;
        }

        public bool TryStartProcess(uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryStartProcess()",
                "bool(uint)", "ProcessManager.cs", 98));
            foreach (var process in _processes)
            {
                if (process.ProcessID == processID)
                {
                    WinttCallStack.RegisterReturn();
                    return TryStartProcess(process.ProcessName);
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }
        public bool TryStopProcess(uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryStopProcess()",
                "bool(uint)", "ProcessManager.cs", 113));
            foreach (var process in _processes)
            {
                if (process.ProcessID == processID)
                {
                    bool isStopped = TryStopProcess(process.ProcessName);
                    WinttCallStack.RegisterReturn();
                    return isStopped;
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        public bool TryGetProcessInstance(out Process process, uint processID)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.TryGetProcessInstance()",
                "Process?(uint)", "ProcessManager.cs", 130));
            foreach (var p in _processes)
            {
                if (p.ProcessID == processID)
                {
                    WinttCallStack.RegisterReturn();
                    process = p;
                    return true;
                }
            }
            WinttCallStack.RegisterReturn();
            process = null;
            return false;
        }

        public IEnumerator<CoroutineControlPoint> UpdateProcesses()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.UpdateProcesses()",
                "IEnumerator<CoroutineControlPoint>()", "ProcessManager.cs", 146));
            while (true)
            {
                for (byte i = 0; i < 3; i++)
                {
                    foreach (var process in _processes)
                    {
                        try
                        {
                            if (process.Type == Process.ProcessType.FromValue(i) && process.IsProcessRunning)
                            {
                                WinttOS.CurrentExecutionSet = process.CurrentSet;

                                process.Update();
                                 
                                if(process.TaskQueue.Count > 1)
                                    process.TaskQueue.Dequeue().Callback();

                                WinttOS.CurrentExecutionSet = wAPI.PrivilegesSystem.PrivilegesSet.HIGHEST;
                            }
                        }
                        catch(Exception e)
                        {
                            WinttDebugger.Error(e.Message, true, this);
                            process.Stop();
                        }
                    }
                }
                WinttCallStack.RegisterReturn();

                yield return null;

                WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.UpdateProcesses()",
                "IEnumerator<CoroutineControlPoint>()", "ProcessManager.cs", 162));

                if (Kernel.IsFinishingKernel)
                {
                    foreach (var process in _processes)
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.ProcessManager.WriteLineProcessesList()",
                "void()", "ProcessManager.cs", 180));
            ConsoleColumnFormatter formatter = new(20, 3);
            formatter.Write("Status");
            formatter.Write("Process Name");
            formatter.Write("PID");

            foreach (var process in _processes)
            {
                formatter.Write(process.IsProcessRunning ? "[X]" : "[ ]");
                formatter.Write(process.ProcessName);
                formatter.Write(process.ProcessID.ToString());
            }
            WinttCallStack.RegisterReturn();
        }
    }
}
