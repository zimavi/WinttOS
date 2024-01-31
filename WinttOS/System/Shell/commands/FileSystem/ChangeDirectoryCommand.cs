using WinttOS.System.Filesystem;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.FileSystem
{
    internal class ChangeDirectoryCommand : Command
    {
        public ChangeDirectoryCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"cd <path\to\folder> - change directory");
        }

        public override string Execute(string[] arguments)
        {
            if (!CurrentPath.Set(arguments[0], out string error))
            {
                return error;
            }
            return null;
        }
    }
}
