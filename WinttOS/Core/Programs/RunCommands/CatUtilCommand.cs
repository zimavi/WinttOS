using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"cat <path\to\file> - reads all text from file (use 'man cat' for more info)");
        }

        public override string execute(string[] arguments)
        {
            CAT instance = new CAT();
            return instance.Execute(arguments);
        }
    }
}
