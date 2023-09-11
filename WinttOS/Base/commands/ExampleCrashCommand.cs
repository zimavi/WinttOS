using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    internal class ExampleCrashCommand : Command
    {
        public ExampleCrashCommand(string name) : base(name, true)
        { }

        public override string execute(string[] arguments)
        {
            exampleStackTraceLogging();
            return base.execute(arguments);
        }

        private void exampleStackTraceLogging()
        {
            throw new Exception("Example stack crash command :)");
        }
    }
}
