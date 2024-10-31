using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public sealed class EnvironmentCommand : Command
    {
        public EnvironmentCommand(string[] commandValues) : base(commandValues)
        {
            CommandManual = new List<string>()
            {
                "NAME",
                "   export - Set environment variables",
                "",
                "SYNOPSIS",
                "   export [var_name]=[var_value]",
                "",
                "DESCRIPTION",
                "   The export command sets the value of an environment variable. It takes",
                "   the form of a variable name followed by and equal sign and the desired",
                "   value.",
                "",
                "OPTIONS",
                "   [var_name]=[var_value]",
                "       Specify the name of the environment variable and the value to",
                "       assign to it. The command expects exactly two parts separated by",
                "       an equal sign.",
                "",
                "EXAMPLES",
                "",
                "   Set an environment variable:",
                "       export PATH=0:\\usr\\bin",
                "",
                "   Set a custom variable:",
                "       export MY_VAR=my_value",
                "",
                "NOTES",
                "   - The command expects exactly two values separated by the '=' character.",
                "   - If the command is no provided in the correct format, an error will",
                "     be returned indicating that exactly two values are expected.",
                "",
                "AUTHOR",
                "   ZImaVI. Developed as part of the WinttOS environment management module."
            };
        }

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
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("export [var_name]=[var_value]");
        }
    }
}
