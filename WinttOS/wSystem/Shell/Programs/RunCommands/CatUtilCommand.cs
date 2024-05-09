using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    //
    // CAT VERISON 0.1.0
    //
    public sealed class CatUtilCommand : Command
    {
        public CatUtilCommand(string[] name) : base(name, false)
        {
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            CAT instance = new CAT();
            SystemIO.STDOUT.PutLine(instance.Execute(arguments.ToArray()));
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- cat {file}");
        }
    }
}
