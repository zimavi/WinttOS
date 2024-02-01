using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class TouchCommand : Command
    {
        public TouchCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("touch <file_name> - creates new empty file");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            File.Create(GlobalData.CurrentDirectory + string.Join(' ', arguments.ToArray()));
            Console.WriteLine("Done.");
            return new(this, ReturnCode.OK);
        }
    }
}
