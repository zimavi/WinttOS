using System;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
        }

        public override ReturnInfo Execute()
        { 

            string CPUbrand = Cosmos.Core.CPU.GetCPUBrandString();
            uint amoundOfRam = Memory.TotalMemory;
            uint usedRam = Memory.GetUsedMemory();
            WinttDebugger.Trace($"Sysinfo => CPUbrand => {CPUbrand}");
            SystemIO.STDOUT.PutLine($"CPU: {CPUbrand}\nAmount of RAM: {amoundOfRam} MB\nUsed RAM: {usedRam /*/ 1000000*/} MB");
            return new(this, ReturnCode.OK);
        }
    }
}
