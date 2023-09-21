//
// Spectial thanks to Aura-OS developers :)
//

using Cosmos.HAL.Drivers.Video.SVGAII;
using System;
using WinttOS.Core.Utils.System;

namespace WinttOS.System.Processing
{
    
    public abstract class Process
    {
        public enum ProcessType
        {
            KernelComponent,
            Driver,
            Service,
            Program,
        }

        /*
        public enum ProcessPriority : byte
        {
            Lowest,
            Lower,
            Normal,
            High,
            Highest
        }
        */

        public string ProcessName { get; protected set; }
        public uint ProcessID { get; protected set; }
        public ProcessType Type { get; protected set; }
        public bool IsProcessInitialized { get; protected set; }
        public bool IsProcessRunning { get; protected set; }
        public bool IsProcessCritical { get; set; } = false;

        public Process(string name, ProcessType type)
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
            if(IsProcessInitialized)
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

    }
}
