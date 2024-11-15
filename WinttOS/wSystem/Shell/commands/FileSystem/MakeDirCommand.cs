using System;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class MakeDirCommand : Command
    {
        public MakeDirCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (!arguments[0].StartsWith('/'))
                arguments[0] = GlobalData.CurrentDirectory + arguments[0];

            GlobalData.FileSystem.CreateDirectory(IOMapper.MapFHSToPhysical(arguments[0]));

            SystemIO.STDOUT.PutLine("Created directory!");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("mkdir [directory]");
        }
    }
}
