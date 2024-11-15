using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class TouchCommand : Command
    {
        public TouchCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (!arguments[0].StartsWith('/'))
                arguments[0] = GlobalData.CurrentDirectory + arguments[0];
            File.Create(IOMapper.MapFHSToPhysical(arguments[0]));
            SystemIO.STDOUT.PutLine("Done.");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage: ");
            SystemIO.STDOUT.PutLine("touch [file]");
        }
    }
}
