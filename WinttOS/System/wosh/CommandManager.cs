using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.System;
using WinttOS.Core;
using WinttOS.System.Users;
using WinttOS.System.wosh.commands;
using WinttOS.System.wosh.commands.FileSystem;
using WinttOS.System.wosh.commands.Misc;
using WinttOS.System.wosh.commands.Screen;
using WinttOS.System.wosh.Programs.RunCommands;
using WinttOS.System.wosh.commands.Processing;
using WinttOS.System.Services;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.System.wosh
{

    public class CommandManager : Service
    {
        private List<Command> commands;
        private bool didRunCycle = true;


        public CommandManager() : base("conman", "wosh.service")
        {
            commands = new List<Command>
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
                new UsersCommand("user"),
                new ProcessCommand("process"),
                //new WgetCommand("wget"),
            };
        }

        /// <summary>
        /// Adds command to command list
        /// </summary>
        /// <param name="command">Command's class that implements <see cref="Command"/> abstract class</param>
        /// <returns>true if successful</returns>
        public bool RegisterCommand(Command command)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.CommandManager.RegisterCommand()",
                "bool()", "WinttOS.cs", 60));
            try
            {
                this.commands.Add(command);
                WinttCallStack.RegisterReturn();
                return true;
            }
            catch (Exception)
            {
                WinttCallStack.RegisterReturn();
                return false;
            }
        }

        public string ProcessInput(string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.CommandManager.ProcessInput()",
                "string(string)", "WinttOS.cs", 77));
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
                if (cmd.CommandName == label)
                {
                    if(cmd.RequiredAccessLevel.Value <= WinttOS.UsersManager.CurrentUser.UserAccess.Value)
                    {
                        try
                        {
                            string result = cmd.Execute(args.ToArray());
                            WinttCallStack.RegisterReturn();
                            return result;
                        }
                        catch (Exception e)
                        {
                            WinttDebugger.Critical("Command crash", true, this);
                            WinttCallStack.RegisterReturn();
                            //return $"Error: {e.GetType()} with message:\n{e.Message}";
                        }
                    }
                    WinttCallStack.RegisterReturn();
                    return "You do not have permission to run this command!";
                }
            }
            WinttCallStack.RegisterReturn();
            return "Command '" + label + "' not exist, please type man <command> or help or more details.";
        }

        public string ProcessInput(ref TempUser user, string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.CommandManager.ProcessInput()",
                "string(ref TempUser, string)", "WinttOS.cs", 121));

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
                if (cmd.CommandName == label)
                {
                    if (cmd.RequiredAccessLevel.Value <= user.UserAccess.Value)
                    {
                        user = null;
                        try
                        {
                            string result = cmd.Execute(args.ToArray());
                            WinttCallStack.RegisterReturn();
                            return result;
                        }
                        catch(Exception e)
                        {
                            WinttCallStack.RegisterReturn();
                            return $"Error: {e.GetType()} with message:\n{e.Message}";
                        }
                    }
                    WinttCallStack.RegisterReturn();
                    return "You do not have permission to run this command!";
                }
            }
            WinttCallStack.RegisterReturn();
            return "Command '" + label + "' not exist, please type man <command> or help or more details.";
        }

        public List<String> GetCommandsList()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.CommandManager.GetCommandsList()",
                "List<string>()", "WinttOS.cs", 167));
            List<String> commands = new List<string>();
            foreach(Command command in this.commands)
            {
                if(!command.IsHiddenCommand)
                    commands.Add(command.CommandName);
            }
            WinttCallStack.RegisterReturn();
            return commands;
        }

        public List<Command> GetCommandsListInstances() => 
            commands;

        public override void ServiceTick()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.CommandManager.ServiceTick()",
                "void()", "WinttOS.cs", 184));
            try
            {
                if (Kernel.IsFinishingKernel)
                {
                    WinttCallStack.RegisterReturn();
                    return;
                }
                if (didRunCycle)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(@$"{WinttOS.UsersManager.CurrentUser.Name}$0:\{GlobalData.CurrentDirectory}> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    didRunCycle = false;
                }
                string input = "";
                bool hasKey = ShellUtils.ProcessExtendedInput(ref input);
                if (hasKey)
                {
                    PowerManagerService.isIdling = false;
                    string[] split = input.Split(' ');
                    Console.ForegroundColor = ConsoleColor.Gray;
                    string response = ProcessInput(input);
                    Console.WriteLine(response);
                    didRunCycle = true;
                }
                else
                    PowerManagerService.isIdling = true;
            }
            #region Catch
            catch
            {
                throw;
            }
            #endregion

            WinttCallStack.RegisterReturn();
        }
    }
}
