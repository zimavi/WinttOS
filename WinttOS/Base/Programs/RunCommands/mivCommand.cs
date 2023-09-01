using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.Programs.RunCommands
{
    public class mivCommand : Command
    {
        public mivCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"miv <path\to\file> - edit file");
        }

        public override string execute(string[] arguments)
        {
            MIV.StartMIV(arguments[0]);
            return String.Empty;
        }
    }
}
