using Cosmos.HAL.Drivers.Video.SVGAII;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinttOS.Base.commands;
using WinttOS.Base.Programs.RunCommands;

namespace WinttOS.Base
{

    public class CommandManager
    {
        private List<Command> commands;

        public CommandManager()
        {
            this.commands = new List<Command>
            {
                new ClearScreenCommand("clear"),
                new EchoCommand("echo"),
                new VerisonCommand("version"),
                new ShutdownCommand("shutdown"),
                new ipconfigCommand("ipconfig"),
                new makefileCommand("mkfile"),
                new makedirCommand("mkdir"),
                new rmCommand("rm"),
                new changeDirectoryCommand("cd"),
                new dirCommand("dir"),
                new SystemInfoCommand("sysinfo"),
                new installCommand("install"),
                new HelpCommand("help"),
                new ManCommand("man"),
                new TimeCommand("time"),
                new DateCommand("date"),
                new TouchCommand("touch"),
            };
        }

        /// <summary>
        /// Adds command to command list
        /// </summary>
        /// <param name="command">Command's class that implements <see cref="Command"/> abstract class</param>
        /// <returns>true if successfull</returns>
        public bool registerCommand(Command command)
        {
            try
            {
                this.commands.Add(command);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string processInput(string input)
        {
            string[] split = input.Split(' ');

            string label = split[0];
            List<String> args = new List<string>();

            int ctr = 0;

            foreach (String i in split)
            {

                if (ctr != 0) args.Add(i);
                ++ctr;
            }

            foreach (Command cmd in this.commands)
            {
                if (cmd.name == label)
                {
                    return cmd.execute(args.ToArray());
                }
            }

            return "Command '" + label + "' not exist, please type man <command> or help or more details.";
        }

        public List<String> getCommandsList()
        {
            List<String> commands = new List<string>();
            foreach(Command command in this.commands)
            {
                if(!command.isHidden)
                    commands.Add(command.name);
            }
            return commands;
        }

        public List<Command> getCommandsListInstances() => commands;
    }
}
