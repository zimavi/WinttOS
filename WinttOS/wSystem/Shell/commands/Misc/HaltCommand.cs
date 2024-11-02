using Cosmos.Core;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public class HaltCommand : Command
    {
        public HaltCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute()
        {
            CPU.DisableInterrupts();
            CPU.Halt();
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("halt");
        }
    }
}
