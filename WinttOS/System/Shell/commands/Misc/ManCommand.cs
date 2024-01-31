using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    public class ManCommand : Command
    {
        public ManCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("man [command name] - shows list of manuals or command manual");
        }

        public override string Execute(string[] arguments)
        {

            if (arguments.Length == 0)
            {
                List<string> commandsWithManuals = new List<string>();
                foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
                {
                    if (command.CommandManual.Any())
                        commandsWithManuals.Add(command.CommandName);

                }
                string returnStr = "List of _commands with manuals:\n";
                returnStr += string.Join('\n', commandsWithManuals.ToArray());
                return returnStr;
            }

            foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                if (command.CommandName == arguments[0])
                {
                    if (command.CommandManual.Any())
                    {
                        return string.Join('\n', command.CommandManual.ToArray());
                    }
                    break;
                }
            }

            return $"There is no manual for {arguments[0]} command!";
        }
    }
}
