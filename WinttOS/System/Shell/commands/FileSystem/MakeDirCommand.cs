using System;
using WinttOS.Core;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.FileSystem
{
    public class MakeDirCommand : Command
    {
        public MakeDirCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"mkdir <dir> - creates new directory in current one");
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length >= 1)
            {
                GlobalData.FileSystem.CreateDirectory(@"0:\" + GlobalData.CurrentDirectory + arguments[0]);
            }
            else
            {
                Console.Write("Enter new dir name: ");
                string dir = Console.ReadLine();
                // Added replacment of spaces in names into _ for preventing unopenable folders
                GlobalData.FileSystem.CreateDirectory(@"0:\" + GlobalData.CurrentDirectory + string.Join('_', dir.Split(' ')));
            }
            return "Created directory!";
        }
    }
}
