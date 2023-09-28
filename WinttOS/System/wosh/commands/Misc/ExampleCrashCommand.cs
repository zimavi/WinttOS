using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.System.wosh;

namespace WinttOS.System.wosh.commands.Misc
{
    internal class ExampleCrashCommand : Command
    {
        public ExampleCrashCommand(string name) : base(name, true, Users.User.AccessLevel.Administrator)
        { }

        public override string Execute(string[] arguments)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.Misc.ExampleCrashCommand.Execute()",
                "string(string[])", "ExampleCrashCommand.cs", 184));

            Kernel.WinttRaiseHardError(WinttStatus.MANUALLY_INITIATED_CRASH, this,
                HardErrorResponseOption.OptionShutdownSystem);

            WinttCallStack.RegisterReturn();
            return base.Execute(arguments);
        }
    }
}
