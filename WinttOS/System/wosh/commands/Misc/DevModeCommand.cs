using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh;

namespace WinttOS.System.wosh.commands.Misc
{
    public class DevModeCommand : Command
    {
        public static bool isInDebugMode { get; private set; }
        public DevModeCommand(string name) : base(name, Users.User.AccessLevel.Administrator)
        {
            CommandManual = new List<string>()
            {
                "By running this command, you can switch dev mode.",
                "This can unlock you some hidden features.",
                "If you add '-i' or '--info' flags after command",
                "you can see in which mode you are."
            };
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length > 0 && (arguments[0] == "-i" || arguments[0] == "--info"))
            {
                return isInDebugMode ? "In debug mode" : "Not in debug mode";
            }
            isInDebugMode = !isInDebugMode;

            if (isInDebugMode)
                return "Now in debug mode";
            else
                return "Now debug mode is off";
        }
    }
}
