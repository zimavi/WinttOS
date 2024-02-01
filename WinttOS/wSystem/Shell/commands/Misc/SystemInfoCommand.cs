using System;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"sysinfo - shows PC info");
        }

        public override ReturnInfo Execute()
        {
            throw new NotImplementedException(nameof(SystemInfoCommand));

            string CPUbrand = Cosmos.Core.CPU.GetCPUBrandString();
            uint amoundOfRam = Cosmos.Core.CPU.GetAmountOfRAM();
            uint usedRam = Cosmos.Core.GCImplementation.GetUsedRAM();
            WinttDebugger.Trace($"Sysinfo => CPUbrand => {CPUbrand}");
            //WinttDebugger.Trace($"Sysinfo => amoundOfRam => {amoundOfRam}");
            //WinttDebugger.Trace($"Sysinfo => usedRam => {usedRam / 1000000}");
            Console.WriteLine($"CPU: {CPUbrand}\nAmount of RAM: {amoundOfRam} MB\nUsed RAM: {usedRam / 1000000} MB");
            return new(this, ReturnCode.OK);
        }
    }
}
