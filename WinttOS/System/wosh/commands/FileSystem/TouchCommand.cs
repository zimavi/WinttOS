using System.IO;
using WinttOS.Core;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.FileSystem
{
    public class TouchCommand : Command
    {
        public TouchCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("touch <file_name> - creates new empty file");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0)
                return "Usage: touch <file_name>";
            File.Create(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments));
            return "Done.";
        }
    }
}
