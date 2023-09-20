using Cosmos.Core.Memory;
using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.Core;
using WinttOS.System.Processing;
using WinttOS.System.Users;
using WinttOS.System.wosh.commands;
using WinttOS.System.wosh.commands.FileSystem;
using WinttOS.System.wosh.commands.Misc;
using WinttOS.System.wosh.commands.Screen;
using WinttOS.System.wosh.Programs.RunCommands;
using WinttOS.System.wosh.commands.Processing;
using WinttOS.System.Services;

namespace WinttOS.System.wosh
{

    public class CommandManager : Service
    {
        private List<Command> commands;
        private bool didRunCycle = true;


        public CommandManager() : base("conman", "wosh.service")
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
                //new installCommand("install"),
                new HelpCommand("help"),
                new ManCommand("man"),
                new TimeCommand("time"),
                new DateCommand("date"),
                new TouchCommand("touch"),
                new SudoCommand("sudo"),
                new WhoAmICommand("whoami"),
                new mivCommand("miv"),
                new CatUtilCommand("cat"),
                new SystemCtlCommand("systemctl"),
                new ProcessCommand("process"),
                //new WgetCommand("wget"),
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
                    if(cmd.RequiredAccessLevel <= WinttOS.UsersManager.currentUser.UserAccess)
                    {
                        try
                        {
                            return cmd.execute(args.ToArray());
                        }
                        catch (Exception e)
                        {
                            return $"Error: {e.GetType()} with message:\n{e.Message}";
                        }
                    }
                    return "You do not have permission to run this command!";
                }
            }

            return "Command '" + label + "' not exist, please type man <command> or help or more details.";
        }

        public string processInput(ref TempUser user, string input)
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
                    if (cmd.RequiredAccessLevel <= user.UserAccess)
                    {
                        user = null;
                        try
                        {
                            return cmd.execute(args.ToArray());
                        }
                        catch(Exception e)
                        {
                            return $"Error: {e.GetType()} with message:\n{e.Message}";
                        }
                    }
                    return "You do not have permission to run this command!";
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

        public override void ServiceTick()
        {
            try
            {
                if (Kernel.FinishingKernel)
                    return;
                if (didRunCycle)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(@$"{WinttOS.UsersManager.currentUser.Name}$0:\{GlobalData.currDir}> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    didRunCycle = false;
                }
                string input = "";
                bool hasKey = ShellUtils.ProcessExtendedThreadableShellInput(ref input);
                if (hasKey)
                {
                    string[] split = input.Split(' ');
                    Console.ForegroundColor = ConsoleColor.Gray;
                    string response = processInput(input);
                    Console.WriteLine(response);
                    didRunCycle = true;
                }
            }
            #region Catch
            catch
            {
                throw;
            }
            #endregion
        }
    }
}
