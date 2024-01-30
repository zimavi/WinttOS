using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core;
using WinttOS.System.Filesystem;
using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.FileSystem
{
    internal class ChangeDirectoryCommand : Command
    {
        public ChangeDirectoryCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"cd <path\to\folder> - change directory");
        }

        public override string Execute(string[] arguments)
        {
            if (!CurrentPath.Set(arguments[0], out string error))
            {
                return error;
            }
            return null;
        }
    }
}
