using System;
using System.Collections.Generic;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    internal class ChangeDirectoryCommand : Command
    {
        public ChangeDirectoryCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"cd <path\to\folder> - change directory");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (!CurrentPath.Set(arguments[0], out string error))
            {
                return new(this, ReturnCode.ERROR, error);
            }
            return new(this, ReturnCode.OK);
        }
    }
}
