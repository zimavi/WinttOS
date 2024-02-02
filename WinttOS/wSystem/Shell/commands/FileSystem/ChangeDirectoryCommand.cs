using System;
using System.Collections.Generic;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    internal class ChangeDirectoryCommand : Command
    {
        public ChangeDirectoryCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (!CurrentPath.Set(arguments[0], out string error))
            {
                return new(this, ReturnCode.ERROR, error);
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- cd {directory}");
        }
    }
}
