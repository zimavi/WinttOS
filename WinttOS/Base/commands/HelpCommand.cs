using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class HelpCommand : Command
    {
        public HelpCommand(string name) : base(name) 
        {
        }

        public override string execute(string[] arguments)
        {
            return HelpCommandManager.getCommandsUsageStringsAsString();
        }
    }
}
