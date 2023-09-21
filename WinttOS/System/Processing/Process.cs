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

        public static string ProcessType_ToString(ProcessType type)
        {
#pragma warning disable CS8524
            return type switch
            {
                ProcessType.KernelComponent => "KernelComponent",
                ProcessType.Driver          => "Driver",
                ProcessType.Service         => "Service",
                ProcessType.Program         => "Program",
            };
#pragma warning restore CS8524
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

        public string Name { get; protected set; }
        public uint ProcessID { get; protected set; }
        public ProcessType Type { get; protected set; }
        public bool Initialized { get; protected set; }
        public bool Running { get; protected set; }
        public bool IsCritical { get; set; } = false;

        public Process(string name, ProcessType type)
        {
            Name = name;
            Type = type;
            ProcessID = 0;
            Initialized = false;
            Running = false;
            if (Type == ProcessType.KernelComponent || Type == ProcessType.Driver)
                IsCritical = true;
        }
        public virtual void Initialize() 
        { 
            if(Initialized) return;
            Initialized = true; 
        }
        public virtual void Start() { if (!Initialized) return; if(Running) return; Running = true; }
        public virtual void Stop() { if (!Initialized) return; if (!Running) return; Running = false; }
        public virtual void Update() { if (!Initialized) return; }

        public void SetName(string name) => Name = name;
        public void SetProcessID(uint processID) => ProcessID = processID;
        public void SetProcessType(ProcessType processType) => Type = processType;

    }
}
