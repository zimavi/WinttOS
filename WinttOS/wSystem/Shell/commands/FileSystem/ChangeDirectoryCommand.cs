using System;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    internal sealed class ChangeDirectoryCommand : Command
    {
        public ChangeDirectoryCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "/")
                arguments[0] = GlobalData.CurrentVolume; // @"0:\";
            if (!CurrentPath.Set(arguments[0], out string error))
            {
                return new(this, ReturnCode.ERROR, error);
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("cd {directory}");
        }
    }
}
