using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    internal class ExampleCrashCommand : Command
    {
        public ExampleCrashCommand(string name) : base(name, true, Users.User.AccessLevel.Administrator)
        { }

        public override string execute(string[] arguments)
        {
            exampleStackTraceLogging(); // Call stack not working :((
            return base.execute(arguments);
        }

        private void exampleStackTraceLogging()
        {
            throw new Exception("Example stack crash command :)");
        }
    }
}
