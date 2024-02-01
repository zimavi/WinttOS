using System.Collections.Generic;
using WinttOS.wSystem.Shell.Utils.Commands;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    public class MivCommand : Command
    {
        public MivCommand(string[] name) : base(name, false)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"miv <path\to\file> - edit file");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            MIV.StartMIV(arguments[0]);
            return new(this, ReturnCode.OK);
        }
    }
}
