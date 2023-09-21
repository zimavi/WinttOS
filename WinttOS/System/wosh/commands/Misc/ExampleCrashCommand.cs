using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh;

namespace WinttOS.System.wosh.commands.Misc
{
    internal class ExampleCrashCommand : Command
    {
        public ExampleCrashCommand(string name) : base(name, true, Users.User.AccessLevel.Administrator)
        { }

        public override string Execute(string[] arguments)
        {
            exampleStackTraceLogging(); // Call stack not working :((
            return base.Execute(arguments);
        }

        private void exampleStackTraceLogging()
        {
            throw new Exception("Example stack crash command :)");
        }
    }
}
