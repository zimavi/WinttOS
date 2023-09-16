using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.Programs.RunCommands
{
    public class mivCommand : Command
    {
        public mivCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"miv <path\to\file> - edit file");
        }

        public override string execute(string[] arguments)
        {
            MIV.StartMIV(arguments[0]);
            return string.Empty;
        }
    }
}
