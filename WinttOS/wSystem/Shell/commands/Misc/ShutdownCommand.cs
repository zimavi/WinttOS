using System;
using System.Collections.Generic;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class ShutdownCommand : Command
    {

        public ShutdownCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"shutdown [-r, -s] - power off pc or reboot it");
            CommandManual = new List<string>()
            {
                "Shutdown command is used for managing power in your PC",
                "It has to have one of two arguments '-s' or '-r'",
                "'-s' argument shuts PC down, and '-r' reboots it"
            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "-s")
            {
                Kernel.ShutdownKernel();
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "-r")
            {
                Kernel.RebootKernel();
                return new(this, ReturnCode.OK);
            }
            else
            {
                return new(this, ReturnCode.OK, "Unknown argument!");
            }
        }
    }
}
