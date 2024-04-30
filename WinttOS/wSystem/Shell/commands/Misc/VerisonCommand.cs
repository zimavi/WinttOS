using System;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class VerisonCommand : Command
    {
        public VerisonCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute()
        {
            Console.WriteLine($"{Kernel.KernelVersion}\n{WinttOS.WinttVersion}:{WinttOS.WinttRevision}\nPowered by Cosmos Kernel");
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- version");
        }
    }
}
