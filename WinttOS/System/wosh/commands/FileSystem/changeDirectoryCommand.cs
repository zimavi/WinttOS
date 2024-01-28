using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core;
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
            if (arguments[0] == null || arguments[0] == string.Empty || arguments[0] == "")
                GlobalData.CurrentDirectory = string.Empty;
            else
                GlobalData.CurrentDirectory = arguments[0] + @"\";

            return $"Changed directory to {arguments[0]}";
        }
    }
}
