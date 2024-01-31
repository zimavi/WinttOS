using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    public class VerisonCommand : Command
    {
        public VerisonCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"version - shows OS version");
        }

        public override string Execute(string[] args)
        {
            return $"{Kernel.KernelVersion}\n{WinttOS.WinttVersion}\nPowered by Cosmos Kernel";
        }
    }
}
