using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class ShutdownCommand : Command
    {

        public ShutdownCommand(String name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"shutdown [-r, -s] - power off pc or reboot it");
            manual = new List<string>()
            {
                "Shutdown command is used for managing power in your PC",
                "It has to have one of two arguments '-s' or '-r'",
                "'-s' argument shuts PC down, and '-r' reboots it"
            };
        }

        public override string execute(string[] arguments)
        {
            if (arguments.Length == 0)
                return "Usage:\n-r - reboot\n-s - shutdown";
            else if (arguments[0] == "-s")
            {
                Cosmos.System.Power.Shutdown();
                return String.Empty;
            }
            else if (arguments[0] == "-r")
            {
                Cosmos.System.Power.Reboot();
                return String.Empty;
            }
            else
                return "Unknown argument!";
        }
    }
}
