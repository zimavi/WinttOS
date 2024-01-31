using WinttOS.System.Shell.Utils.Commands;

namespace WinttOS.System.Shell.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string name) : base(name, false)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"cat <path\to\file> - reads all text from file (use 'man cat' for more info)");
        }

        public override string Execute(string[] arguments)
        {
            CAT instance = new CAT();
            return instance.Execute(arguments);
        }
    }
}
