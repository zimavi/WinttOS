using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class DevModeCommand : Command
    {
        public static bool IsInDebugMode { get; private set; }
        public DevModeCommand(string[] name) : base(name, User.AccessLevel.Administrator)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "-i" || arguments[0] == "--info")
            {
                SystemIO.STDOUT.PutLine(IsInDebugMode ? "In debug mode" : "Not in debug mode");
                return new(this, ReturnCode.OK);
            }
            return new(this, ReturnCode.ERROR_ARG, "Flag expected!");
        }

        public override ReturnInfo Execute()
        {
            IsInDebugMode = !IsInDebugMode;

            if (IsInDebugMode)
                SystemIO.STDOUT.PutLine("Now in debug mode");
            else
                SystemIO.STDOUT.PutLine("Now debug mode is off");

            return new(this, ReturnCode.OK);
        }
    }
}
