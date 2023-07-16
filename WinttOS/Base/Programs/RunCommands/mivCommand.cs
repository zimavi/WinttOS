using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.MIV;

namespace WinttOS.Base.Programs.RunCommands
{
    public class mivCommand : Command
    {
        public mivCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            MIV.MIV.StartMIV(arguments[0]);
            return String.Empty;
        }
    }
}
