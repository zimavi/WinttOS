using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public class RebootCommand : Command
    {
        public RebootCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute()
        {
            Kernel.RebootKernel();
            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage: ");
            SystemIO.STDOUT.PutLine("reboot");
        }
    }
}
