//
// ORIGINAL CODE: https://github.com/aura-systems/Aura-Operating-System/tree/master
// I ONLY MODIFIED IT 
//


using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.API;
using static WinttOS.wSystem.API.PrivilegesSystem;
using WinttOS.wSystem.Users;
using System.Collections.Generic;
using System;

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
        public ProcessType Type { get; private set; }
            = ProcessType.Program;
        public PrivilegesSet CurrentSet { get; private set; } 
            = PrivilegesSet.DEFAULT;
        public bool IsProcessInitialized { get; private set; }
        public bool IsProcessRunning { get; private set; }
        public bool IsProcessCritical { get; set; } = false;
        public Process OwnerProcess { get; private set; }
        public bool HasOwnerProcess => !OwnerProcess.IsNull();
        public Queue<Action> TaskQueue { get; private set; } = new Queue<Action>();

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
            }
            catch (Exception ex)
            {
                WinttDebugger.Error(ex.Message, true, this);
            }
        }
        public virtual void Initialize()
        {
            if (IsProcessInitialized)
                return;

            IsProcessInitialized = true;
        }
        public virtual void Start() { if (!IsProcessInitialized) return; if(IsProcessRunning) return; IsProcessRunning = true; }
        public virtual void Stop() { if (!IsProcessInitialized) return; if (!IsProcessRunning) return; IsProcessRunning = false; }
        public virtual void Update() { if (!IsProcessInitialized) return; }

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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Processing.Process.TryRisePrivileges",
                "bool(PrivilegesSet)", "Process.cs", 71));
            if (WinttOS.UsersManager.CurrentUser.UserAccess.Value >= User.AccessLevel.Guest.Value &&
                WinttOS.UsersManager.CurrentUser.UserAccess.PrivilegeSet.Privileges <= requested_type.Privileges && 
                !HasOwnerProcess)
            {
                CurrentSet = requested_type;
                WinttCallStack.RegisterReturn();
                return true;
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        protected void SetChild(Process ChildOwner)
        {
            if (OwnerProcess == ChildOwner)
                return;
            ChildOwner.OwnerProcess = this;
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
    }
}
