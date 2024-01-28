using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class VerisonCommand : Command
    {
        public VerisonCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"version - shows OS version");
        }

        public override string Execute(string[] args)
        {
            return $"{Kernel.KernelVersion}\n{WinttOS.WinttVersion}\nPowered by Cosmos Kernel";
        }
    }
}
