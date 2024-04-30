using System;
using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public sealed class EnvironmentCommand : Command
    {
        public EnvironmentCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            string[] exportcmd = arguments[0].Split('=');

            if (exportcmd.Length != 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
            }

            string var = exportcmd[0];
            string value = exportcmd[1];

            wAPI.Environment.SetEnvironmentVariable(var, value);

            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- export {var_name} {var_value}");
        }
    }
}
