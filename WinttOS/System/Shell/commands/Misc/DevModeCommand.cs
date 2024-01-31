using System;
using System.Collections.Generic;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    public class DevModeCommand : Command
    {
        public static bool IsInDebugMode { get; private set; }
        public DevModeCommand(string name) : base(name, User.AccessLevel.Administrator)
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
                return IsInDebugMode ? "In debug mode" : "Not in debug mode";
            }
            IsInDebugMode = !IsInDebugMode;

            if (IsInDebugMode)
                return "Now in debug mode";
            else
                return "Now debug mode is off";
        }
    }
}
