using System.IO;
using WinttOS.Core;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.FileSystem
{
    public class TouchCommand : Command
    {
        public TouchCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("touch <file_name> - creates new empty file");
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
