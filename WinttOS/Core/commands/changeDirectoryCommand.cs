using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    internal class changeDirectoryCommand : Command
    {
        public changeDirectoryCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"cd <path\to\folder> - change directory");
        }

        public override string execute(string[] arguments)
        {
            if (arguments[0] == null || arguments[0] == String.Empty || arguments[0] == "")
                GlobalData.currDir = string.Empty;
            else
                GlobalData.currDir = arguments[0] + @"\";

            return $"Changed directory to {arguments[0]}";
        }
    }
}
