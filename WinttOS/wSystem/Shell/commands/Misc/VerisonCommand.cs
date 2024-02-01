using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class VerisonCommand : Command
    {
        public VerisonCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"version - shows OS version");
        }

        public override ReturnInfo Execute()
        {
            Console.WriteLine($"{Kernel.KernelVersion}\n{WinttOS.WinttVersion}\nPowered by Cosmos Kernel");
            return new(this, ReturnCode.OK);
        }
    }
}
