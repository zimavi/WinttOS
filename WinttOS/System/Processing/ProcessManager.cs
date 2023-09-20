﻿using Cosmos.System.Coroutines;
using System.Collections.Generic;
using WinttOS;

namespace WinttOS.System.Processing
{
    public class ProcessManager
    {
        List<Process> _processes = new();

        public uint ProcessesCount => (uint) _processes.Count;
        public uint LastProcessID => (uint) _processes.Count - 1; 

        public bool RegisterProcess(Process process)
        {
            foreach (var _process in _processes)
            {
                if (_process == process) return false;
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].Initialize();
            return true;
        }
        public bool RegisterProcess(Process process, ref uint newProcessID)
        {
            foreach (var _process in _processes)
            {
                if (_process == process) return false;
            }
            _processes.Add(process);
            _processes[_processes.Count - 1].SetProcessID((uint)_processes.Count - 1);
            _processes[_processes.Count - 1].Initialize();
            newProcessID = (uint)_processes.Count - 1;
            return true;
        }

        public bool StartProcess(string processName)
        {
            foreach (var process in _processes)
            {
                if (process.Name == processName) 
                {
                    if (process.Running)
                        return false;
                    process.Start(); 
                    return true; 
                }
            }
            return false;
        }

        public bool StopProcess(string processName)
        {
            foreach (var process in _processes)
            {
                if(process.Name == processName)
                {
                    if (!process.Running)
                        return false;
                    process.Stop();
                    return true;
                }    
            }
            return false;
        }

        public bool StartProcess(uint processID)
        {
            foreach (var process in _processes)
            {
                if (process.ProcessID == processID)
                    return StartProcess(process.Name);
            }
            return false;
        }
        public bool StopProcess(uint processID)
        {
            foreach (var process in _processes)
            {
                 if(process.ProcessID == processID)
                    return StopProcess(process.Name);
            }
            return false;
        }

        public Process? GetProcessInstance(uint processID)
        {
            foreach(var process in _processes)
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
                    foreach (var process in _processes)
                    {
                        if (process.Type == (Process.ProcessType) i && process.Running)
                            process.Update();
                    }
                }
                yield return null;

                if (Kernel.FinishingKernel)
                {
                    foreach (var process in _processes)
                    {
                        if (process.Running)
                            process.Stop();
                    }
                    break;
                }
            }
        }

        public string GetProcessesList()
        {
            string result = "Status\tName\tProcess ID\n";

            foreach (var process in _processes)
            {
                if (process.Running)
                    result += $" [X]\t{process.Name}\t{process.ProcessID}\n";
                else
                    result += $" [ ]\t{process.Name}\t{process.ProcessID}\n";
            }

            return result;
        }
    }
}
