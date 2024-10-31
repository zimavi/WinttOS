using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Users;

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

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, process.ProcessName);
            ShellUtils.MoveCursorUp();

            foreach (var p in _processes)
            {
                if (p == process)
                {
                    ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.FAILED, process.ProcessName);
                    return false;
                }
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].CurrentSet = UsersManager.LoggedLevel.PrivilegeSet;
            _processes[_processes.Count - 1].Initialize();

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, process.ProcessName);

            return true;
        }
        public bool TryRegisterProcess(Process process, out uint newProcessID)
        {

            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, process.ProcessName);
            ShellUtils.MoveCursorUp();

            foreach (var _process in _processes)
            {
                if (_process == process)
                {
                    ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.FAILED, process.ProcessName);
                    newProcessID = 0;
                    return false;
                }
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].CurrentSet = UsersManager.LoggedLevel.PrivilegeSet;
            _processes[_processes.Count - 1].Initialize();
            newProcessID = (uint)_processes.Count - 1;
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, process.ProcessName);
            return true;
        }

        public bool TryStartProcess(string processName)
        {
            return TryStartProcess(_processes.Find(p => p.ProcessName == processName).ProcessID);
        }

        public bool TryStopProcess(string processName)
        {
            return TryStopProcess(_processes.Find(p => p.ProcessName == processName).ProcessID);
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

            foreach(Process p in _processes)
            {
                if(p.ProcessID == processId)
                {
                    if(!p.IsProcessRunning)
                    {
                        _processes.Remove(p);
                        return true;
                    }

                    break;
                }
            }

            return false;
        }

        public bool TryStartProcess(uint processID)
        {
            ShellUtils.PrintTaskResult("Starting", ShellTaskResult.DOING, "PID " + processID.ToString());
            ShellUtils.MoveCursorUp();

            foreach (var process in _processes)
            {
                if (process.ProcessID == processID)
                {
                    if (process.IsProcessRunning)
                    {
                        ShellUtils.PrintTaskResult("Starting", ShellTaskResult.FAILED, process.ProcessName);
                        return false;
                    }
                    ShellUtils.PrintTaskResult("Starting", ShellTaskResult.OK, process.ProcessName);
                    process.Start();
                    return true;
                }
            }
            ShellUtils.PrintTaskResult("Starting", ShellTaskResult.FAILED, "PID " + processID.ToString() + " not found");
            return false;
        }
        public bool TryStopProcess(uint processID)
        {
            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.DOING, "PID " + processID.ToString());
            ShellUtils.MoveCursorUp();
            foreach (var process in _processes)
            {
                if (process.ProcessID == processID)
                {
                    if (!process.IsProcessRunning)
                    {
                        ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, process.ProcessName);
                        return false;
                    }
                    process.Stop();
                    ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.OK, process.ProcessName);
                    return true;
                }
            }
            ShellUtils.PrintTaskResult("Stopping", ShellTaskResult.FAILED, "PID " + processID.ToString() + " not found");
            return false;
        }

        public bool TryGetProcessInstance(out Process process, uint processID)
        {
            foreach (var p in _processes)
            {
                if (p.ProcessID == processID)
                {
                    process = p;
                    return true;
                }
            }
            process = null;
            return false;
        }

        public IEnumerator<CoroutineControlPoint> UpdateProcesses()
        {
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
                            Logger.DoOSLog("[Warn] Process '" + process.ProcessName + "' (PID " + process.ProcessID + ") threw excpetion: " + e.Message);
                            process.Stop();
                        }
                    }
                }

                yield return null;


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
        }

        public void WriteLineProcessesList()
        {
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
        }
    }
}
