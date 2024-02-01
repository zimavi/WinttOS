using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class MakeFileCommand : Command
    {
        public MakeFileCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"mkfile <new.file> - creates new file");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            var file_stream = File.Create(@"0:\" + GlobalData.CurrentDirectory + @"\" + string.Join(' ', arguments));
            

            Console.WriteLine("Created file!");
            return new(this, ReturnCode.OK);
        }

        public override ReturnInfo Execute()
        {
            Console.Write("Enter file name: ");
            string file = Console.ReadLine();
            // Added replacement of spaces in names into _ for preventing unopenable files
            var file_stream = File.Create(@"0:\" + GlobalData.CurrentDirectory + @"\" + string.Join('\n', file.Split(' ')));
            return new(this, ReturnCode.OK);
        }
    }
}
