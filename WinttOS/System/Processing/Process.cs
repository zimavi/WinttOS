//
// Special thanks to Aura-OS developers :)
//


using WinttOS.Core.Utils.System;

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

    }
}
