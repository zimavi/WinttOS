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
            this.commands = new List<Command>(1);
            this.commands.Add(new ClearScreenCommand("clear"));
            this.commands.Add(new EchoCommand("echo"));
            this.commands.Add(new VerisonCommand("version"));
            this.commands.Add(new ShutdownCommand("shutdown"));
            this.commands.Add(new ipconfigCommand("ipconfig"));
            this.commands.Add(new makefileCommand("mkfile"));
            this.commands.Add(new makedirCommand("mkdir"));
            this.commands.Add(new rmCommand("rm"));
            this.commands.Add(new changeDirectoryCommand("cd"));
            this.commands.Add(new dirCommand("dir"));
            this.commands.Add(new SystemInfoCommand("sysinfo"));
            this.commands.Add(new WriteFileCommand("wrfile"));
            this.commands.Add(new mivCommand("miv"));
            //this.commands.Add(new loadUiCommand("load_ui"));
            this.commands.Add(new installCommand("install"));
        }

        public string processInputExample(string input)
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

            return "Command '" + label + "' not exist, please type man <command> or help <command> or more details.";
        }
    }
}
