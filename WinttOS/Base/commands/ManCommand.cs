using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class ManCommand : Command
    {
        public ManCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager("man [command name] - shows list of manuals or command manual");
        }

        public override string execute(string[] arguments)
        {
            //if(arguments.Length == 0)
            //{
            //    string returnStr = "All registrated manuals: \n";
            //    returnStr += String.Join('\n', ManCommandManager.getAllManuals().GetAllKeys().ToArray());
            //    return returnStr;
            //}
            //if (ManCommandManager.getAllManuals().Keys.FirstOrDefault(str => str.Contains(arguments[0])) != null)
            //{
            //    return ManCommandManager.getCommandManual(arguments[0]);
            //}

            if(arguments.Length == 0)
            {
                List<string> commandsWithManuals = new List<string>();
                foreach(Command command in Kernel.manager.getCommandsListInstances())
                {
                    if (command.manual.Any())
                        commandsWithManuals.Add(command.name);
                }
                string returnStr = "List of commands with manuals:\n";
                returnStr += String.Join('\n', commandsWithManuals.ToArray());
                return returnStr;
            }

            foreach(Command command in Kernel.manager.getCommandsListInstances())
            {
                if(command.name == arguments[0])
                {
                    if(command.manual.Any())
                    {
                        return String.Join('\n', command.manual.ToArray());
                    }
                    break;
                }
            }

            return $"There is no manual for {arguments[0]} command!";
        }
    }
}
