using System;
using System.Collections.Generic;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class DevModeCommand : Command
    {
        public static bool IsInDebugMode { get; private set; }
        public DevModeCommand(string[] name) : base(name, User.AccessLevel.Administrator)
        {
            CommandManual = new List<string>()
            {
                "By running this command, you can switch dev mode.",
                "This can unlock you some hidden features.",
                "If you add '-i' or '--info' flags after command",
                "you can see in which mode you are."
            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "-i" || arguments[0] == "--info")
            {
                Console.WriteLine(IsInDebugMode ? "In debug mode" : "Not in debug mode");
                return new(this, ReturnCode.OK);
            }
            return new(this, ReturnCode.ERROR_ARG, "Flag expected!");
        }

        public override ReturnInfo Execute()
        {
            IsInDebugMode = !IsInDebugMode;

            if (IsInDebugMode)
                Console.WriteLine("Now in debug mode");
            else
                Console.WriteLine("Now debug mode is off");

            return new(this, ReturnCode.OK);
        }
    }
}
