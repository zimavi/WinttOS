using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class MakeFileCommand : Command
    {
        public MakeFileCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            var file_stream = File.Create(GlobalData.CurrentDirectory + @"\" + string.Join(' ', arguments));
            

            Console.WriteLine("Created file!");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- mkfile {file}");
        }
    }
}
