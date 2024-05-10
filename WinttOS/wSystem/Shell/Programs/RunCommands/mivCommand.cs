using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.Programs.RunCommands
{
    public sealed class MivCommand : Command
    {
        public MivCommand(string[] name) : base(name, false)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            MIV.StartMIV(arguments[0]);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- miv {file}");
        }
    }
}
