using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class SystemInfoCommand : Command
    {
        public SystemInfoCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"sysinfo - shows PC info");
        }

        public override string execute(string[] arguments)
        {
            string CPUbrand = Cosmos.Core.CPU.GetCPUBrandString();
            uint amoundOfRam = Cosmos.Core.CPU.GetAmountOfRAM();
            uint usedRam = Cosmos.Core.GCImplementation.GetUsedRAM() * 1000000;
            return $"CPU: {CPUbrand}\nAmount of RAM: {amoundOfRam} MB\nUsed RAM: {usedRam} MB";
        }
    }
}
