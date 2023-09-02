using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class VerisonCommand : Command
    {
        public VerisonCommand(string name) : base(name, false) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"version - shows OS version");
            manual = new List<string>()
            {
                "Version command will return version of OS"
            };
        }

        public override string execute(string[] args)
        {
            return $"WinttOS v.0.1.0-dev build 387\nPowered by Cosmos Kernel";
        }
    }
}
