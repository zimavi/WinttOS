using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class TouchCommand : Command
    {
        public TouchCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            File.Create(GlobalData.CurrentDirectory + string.Join(' ', arguments.ToArray()));
            SystemIO.STDOUT.PutLine("Done.");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage: ");
            SystemIO.STDOUT.PutLine("- touch {file}");
        }
    }
}
