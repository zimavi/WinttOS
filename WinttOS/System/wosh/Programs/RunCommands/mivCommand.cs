using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.Programs.RunCommands
{
    public class MivCommand : Command
    {
        public MivCommand(string name) : base(name, false)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"miv <path\to\file> - edit file");
        }

        public override string Execute(string[] arguments)
        {
            MIV.StartMIV(arguments[0]);
            return string.Empty;
        }
    }
}
