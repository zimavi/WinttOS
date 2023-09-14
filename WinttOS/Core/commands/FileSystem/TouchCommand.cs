using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands.FileSystem
{
    public class TouchCommand : Command
    {
        public TouchCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("touch <file_name> - creates new empty file");
        }

        public override string execute(string[] arguments)
        {
            if (arguments.Length == 0)
                return "Usage: touch <file_name>";
            File.Create(@"0:\" + GlobalData.currDir + string.Join(' ', arguments));
            return "Done.";
        }
    }
}
