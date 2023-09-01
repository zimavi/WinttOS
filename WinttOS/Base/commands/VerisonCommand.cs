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
        public VerisonCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"version - shows OS version");
        }

        public override string execute(string[] args)
        {
            return $"WinttOS v.0.0.1\nMade with Cosmos Kernel";
        }
    }
}
