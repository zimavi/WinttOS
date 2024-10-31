using System;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class VerisonCommand : Command
    {
        public VerisonCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute()
        {
            SystemIO.STDOUT.PutLine($"{Kernel.KernelVersion}\n{WinttOS.WinttVersion}:{WinttOS.WinttRevision}\nPowered by Cosmos Kernel");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("version");
        }
    }
}
