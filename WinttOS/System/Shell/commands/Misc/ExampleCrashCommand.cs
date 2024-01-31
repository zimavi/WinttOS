using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    internal class ExampleCrashCommand : Command
    {
        public ExampleCrashCommand(string name) : base(name, true, User.AccessLevel.Administrator)
        { }

        public override string Execute(string[] arguments)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.Misc.ExampleCrashCommand.Execute()",
                "string(string[])", "ExampleCrashCommand.cs", 184));

            Kernel.WinttRaiseHardError(WinttStatus.MANUALLY_INITIATED_CRASH, this,
                HardErrorResponseOption.OptionShutdownSystem);

            WinttCallStack.RegisterReturn();
            return base.Execute(arguments);
        }
    }
}
