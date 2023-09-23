﻿using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"sysinfo - shows PC info");
        }

        public override string Execute(string[] arguments)
        {
            string CPUbrand = Cosmos.Core.CPU.GetCPUBrandString();
            uint amoundOfRam = Cosmos.Core.CPU.GetAmountOfRAM();
            uint usedRam = Cosmos.Core.GCImplementation.GetUsedRAM() * 1000000;
            return $"CPU: {CPUbrand}\nAmount of RAM: {amoundOfRam} MB\nUsed RAM: {usedRam} MB";
        }
    }
}