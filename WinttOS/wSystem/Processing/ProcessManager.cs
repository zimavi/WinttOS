using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Registry;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Processing
{

    public sealed class ProcessManager
    {
        private List<Process> _processes = new();

        public List<Process> Processes => _processes;

        public uint ProcessesCount => (uint) _processes.Count;
        
        public int CurrentProcessID { get; internal set; }

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

            Logger.DoOSLog("[Info] Registering process ");

            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].CurrentSet = UsersManager.LoggedLevel.PrivilegeSet;
            _processes[_processes.Count - 1].Initialize();
            newProcessID = (uint)_processes.Count - 1;
            Logger.DoOSLog("[Info] Initialized process, creating environment");

            List<EnvKey> env = new List<EnvKey>
            {
                new("USER", UsersManager.userLogged ?? "root"),
                new("TMPDIR", @"0:\proc\" + UUID.UUIDToString(_processes[_processes.Count - 1].ProcessUUID) + "\\"),
                new("PWD", GlobalData.CurrentDirectory),
                new("SHELL", "ShellX")
            };
                

            Registry.Environment.PerProcessEnvironment.Add((int)_processes[_processes.Count - 1].ProcessID, env);

            Logger.DoOSLog("[Info] Initialized process '" + process.ProcessName + "' (PID: " + process.ProcessID + ", UUID: " + UUID.UUIDToString(process.ProcessUUID));

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
                                CurrentProcessID = (int)process.ProcessID;

                                WinttOS.CurrentExecutionSet = process.CurrentSet;

                                process.Update();
                                 
                                if(process.TaskQueue.Count > 1)
                                    process.TaskQueue.Dequeue().Callback();

                                WinttOS.CurrentExecutionSet = wAPI.PrivilegesSystem.PrivilegesSet.HIGHEST;

                                CurrentProcessID = -1;
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

        public void PrintProcessTree(Process presentProcess, string indent = "", bool isLastChild = true)
        {
            SystemIO.STDOUT.PutLine(indent + (isLastChild ? GlobalData.TREE_PART1 : GlobalData.TREE_PART3) + presentProcess.ProcessName + " (PID: " + presentProcess.ProcessID + ")");

            indent += isLastChild ? "    " : GlobalData.TREE_PART2;

            var children = presentProcess.ChildProcesses;
            for(int i = 0; i < children.Count; i++)
            {
                PrintProcessTree(children[i], indent, i == children.Count - 1);
            }
        }
        public List<Process> GetRootProcesses()
        {
            List<Process> rootProcesses = new();

            foreach (var process in _processes)
            {
                if (!process.HasOwnerProcess)
                {
                    rootProcesses.Add(process);
                }
            }

            return rootProcesses;
        }
    }
}
