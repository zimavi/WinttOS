//
// ORIGINAL CODE: https://github.com/aura-systems/Aura-Operating-System/tree/master
// 

using System;
using System.Collections.Generic;
using System.Drawing;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.wAPI.PrivilegesSystem;

namespace WinttOS.wSystem.Processing
{
    // TODO: Make privilege system
    public abstract class Process
    {
        public sealed class ProcessType : SmartEnum<ProcessType, byte>
        {
            public static readonly ProcessType KernelComponent = new("KernelComponent", 0);
            public static readonly ProcessType Driver = new("Driver", 1);
            public static readonly ProcessType Service = new("Service", 2);
            public static readonly ProcessType Program = new("Program", 3);
            private ProcessType(string name, byte value) : base(name, value)
            { }
        }

        public string ProcessName { get; protected set; }
        public uint ProcessID { get; private set; }
        public byte[] ProcessUUID { get; private set; }
        public ProcessType Type { get; private set; }
            = ProcessType.Program;
        public PrivilegesSet CurrentSet { get; internal set; } 
            = PrivilegesSet.DEFAULT;
        public bool IsProcessInitialized { get; private set; }
        public bool IsProcessRunning { get; private set; }
        public bool IsProcessCritical { get; set; } = false;
        public Process OwnerProcess { get; private set; }
        public bool HasOwnerProcess => !OwnerProcess.IsNull();
        public Queue<Task> TaskQueue { get; private set; } = new Queue<Task>();
        public List<Process> ChildProcesses { get; private set; } = new List<Process>();

        protected Process(string name, ProcessType type)
        {
            try
            {
                ProcessName = name;
                Type = type;
                ProcessID = 0;
                IsProcessInitialized = false;
                IsProcessRunning = false;
                if (type == ProcessType.KernelComponent || type == ProcessType.Driver)
                    IsProcessCritical = true;
                ProcessUUID = UUID.GenerateUUID();
            }
            catch (Exception ex)
            {
                Logger.DoOSLog("[ERROR] " + ex.Message);
            }
        }
        public virtual void Initialize()
        {
            if (IsProcessInitialized)
                return;

            IsProcessInitialized = true;
        }
        public virtual void Start() 
        { 
            if (!IsProcessInitialized) return; 
            if(IsProcessRunning) return;

            IsProcessRunning = true; 
        }
        public virtual void Stop() 
        { 
            if (!IsProcessInitialized) return; 
            if (!IsProcessRunning) return; 

            IsProcessRunning = false; 

        }
        public virtual void Update() 
        { 
            if (!IsProcessInitialized) return; 
            if (!IsProcessRunning) return; 
        }

        public void SetName(string name) => 
            ProcessName = name;
        public void SetProcessID(uint processID) => 
            ProcessID = processID;
        public void SetProcessType(ProcessType processType) => 
            Type = processType;


        // API FUNCTIONS


        /// <summary>
        /// Request to rise process privileges.
        /// Won't raise, if has OwnerProcess. In this case only Owner can grant its privileges
        /// </summary>
        /// <param name="requested_type">Requested privileges set</param>
        /// <returns><see langword="true"/> if privileges raised, otherwise, <see langword="false"/></returns>
        protected bool TryRisePrivileges(PrivilegesSet requested_type)
        {
            if (UsersManager.LoggedLevel.Value >= AccessLevel.Default.Value &&
                UsersManager.LoggedLevel.PrivilegeSet.Privileges <= requested_type.Privileges && 
                !HasOwnerProcess)
            {
                CurrentSet = requested_type;
                return true;
            }
            return false;
        }

        protected void SetChild(Process child)
        {
            if (OwnerProcess == child || child.HasOwnerProcess)
                return;
            child.OwnerProcess = this;
            ChildProcesses.Add(child);
        }

        protected bool TrySetChildPrivileges(Process child, PrivilegesSet requested_type)
        {
            if(requested_type.Value <= CurrentSet.Value)
            {
                child.CurrentSet = requested_type;
                return true;
            }

            return false;
        }

        protected bool HasEnvironmentValue(string name)
        {
            return Registry.Environment.HasProcessEnvValue((int)ProcessID, name);
        }

        protected void SetEnvironmentValue(string name, string value)
        {
            Registry.Environment.SetProcessEnvValue((int)ProcessID, name, value);
        }

        protected string GetEnvironmentValue(string name)
        {
            return Registry.Environment.GetProcessEnvValue((int)ProcessID, name);
        }

        protected void Exit(int exitCode)
        {
            if(exitCode != 0)
            {
                if (WinttOS.IsTty)
                {
                    Color bg = WinttOS.Tty.BackgroundColor;
                    WinttOS.Tty.BackgroundColor = Color.Red;

                    WinttOS.Tty.WriteLine(ProcessID + " PID -> Error!");

                    WinttOS.Tty.BackgroundColor = bg;
                }
                else
                {
                    ConsoleColor bg = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.Red;

                    Console.WriteLine(ProcessID + " PID -> Error!");

                    Console.BackgroundColor = bg;
                }
            }

            WinttOS.ProcessManager.TryStopProcess(ProcessID);
        }
    }
}
