using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class TouchCommand : Command
    {
        public TouchCommand(string name) : base(name, false)
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
