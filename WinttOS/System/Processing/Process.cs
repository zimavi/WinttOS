//
// ORIGINAL CODE: https://github.com/aura-systems/Aura-Operating-System/tree/master
// I ONLY MODIFIED IT 
//


using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.API;
using static WinttOS.System.API.PrivilegesSystem;
using WinttOS.System.Users;

namespace WinttOS.System.Processing
{
    // TODO: Make privilege system
    public abstract class Process
    {
        public sealed class ProcessType : SmartEnum<ProcessType>
        {
            public static readonly ProcessType KernelComponent = new("KernelComponent", 0);
            public static readonly ProcessType Driver = new("Driver", 1);
            public static readonly ProcessType Service = new("Service", 2);
            public static readonly ProcessType Program = new("Program", 3);
            private ProcessType(string name, int value) : base(name, value)
            { }
        }

        public string ProcessName { get; protected set; }
        public uint ProcessID { get; private set; }
        public ProcessType Type { get; private set; }
        public PrivilegesSet CurrentSet { get; private set; } 
            = PrivilegesSet.DEFAULT;
        public bool IsProcessInitialized { get; private set; }
        public bool IsProcessRunning { get; private set; }
        public bool IsProcessCritical { get; set; } = false;

        protected Process(string name, ProcessType type)
        {
            ProcessName = name;
            Type = type;
            ProcessID = 0;
            IsProcessInitialized = false;
            IsProcessRunning = false;
            if (Type == ProcessType.KernelComponent || Type == ProcessType.Driver)
                IsProcessCritical = true;
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
        /// </summary>
        /// <param name="requested_type">Requested privileges set</param>
        /// <returns><see langword="true"/> if privileges raised, otherwise, <see langword="false"/></returns>
        public bool RisePrivileges(PrivilegesSet requested_type)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Processing.Process.RisePrivileges",
                "bool(PrivilegesSet)", "Process.cs", 71));
            if (WinttOS.UsersManager.CurrentUser.UserAccess.Value >= User.AccessLevel.Guest.Value &&
                WinttOS.UsersManager.CurrentUser.UserAccess.PrivilegeSet.Privileges <= requested_type.Privileges)
            {
                CurrentSet = requested_type;
                WinttCallStack.RegisterReturn();
                return true;
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

    }
}
