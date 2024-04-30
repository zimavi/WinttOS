using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.wAPI.Events
{
    public sealed class SystemEvents : SmartEnum<SystemEvents>
    {

        public static readonly SystemEvents SYS_INIT_COMPLETE = new("_EVENT_SYS_INIT_COMPLETE", 0);


        public SystemEvents(string name, int value) : base(name, value)
        {}
    }
}
