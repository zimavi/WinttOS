using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class ManCommand : Command
    {
        public ManCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                if (command.CommandValues[0] == arguments[0])
                {
                    if (command.CommandManual.Any())
                    {
                        SystemIO.STDOUT.PutLine(string.Join('\n', command.CommandManual.ToArray()));
                        return new(this, ReturnCode.OK);
                    }
                    break;
                }
            }

            return new(this, ReturnCode.ERROR, $"There is no manual for {arguments[0]} command!");
        }

        public override ReturnInfo Execute()
        {
            List<string[]> commandsWithManuals = new List<string[]>();
            foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                if (command.CommandManual.Any())
                    commandsWithManuals.Add(command.CommandValues);

            }
            string returnStr = "List of _commands with manuals:\n";
            returnStr += string.Join('\n', commandsWithManuals.ToArray()[0]);
            SystemIO.STDOUT.PutLine(returnStr);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- man [command name]");
        }
    }
}
