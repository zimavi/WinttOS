using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Sys;
using WinttOS.Core;
using WinttOS.System.Users;
using WinttOS.System.Shell.Commands.FileSystem;
using WinttOS.System.Shell.Commands.Misc;
using WinttOS.System.Shell.Commands.Screen;
using WinttOS.System.Shell.Programs.RunCommands;
using WinttOS.System.Shell.Commands.Processing;
using WinttOS.System.Services;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.Shell.Utils;
using WinttOS.System.Shell.Commands.Networking;
using WinttOS.System.Shell.Commands.Users;

namespace WinttOS.System.Shell
{

    public class CommandManager : Service
    {
        private List<Command> _commands;
        private bool _didRunCycle = true;


        public CommandManager() : base("WoshDaemon", "WoshManagerDaemon")
        {
            _commands = new List<Command>
            {
                new ClearScreenCommand("clear"),
                new EchoCommand("echo"),
                new VerisonCommand("version"),
                new ShutdownCommand("shutdown"),
                new IpConfigCommand("ipconfig"),
                new MakeFileCommand("mkfile"),
                new MakeDirCommand("mkdir"),
                new RmCommand("rm"),
                new ChangeDirectoryCommand("cd"),
                new DirCommand("dir"),
                //new SystemInfoCommand("sysinfo"),
                //new installCommand("install"),
                new HelpCommand("help"),
                new ManCommand("man"),
                new TimeCommand("time"),
                new TouchCommand("touch"),
                new SudoCommand("sudo"),
                new WhoAmICommand("whoami"),
                new MivCommand("miv"),
                new CatUtilCommand("cat"),
                new SystemCtlCommand("systemctl"),
                new UsersCommand("_user"),
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.RegisterCommand()",
                "bool()", "WinttOS.cs", 60));
            try
            {
                this._commands.Add(command);
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.ProcessInput()",
                "string(string)", "WinttOS.cs", 77));

            List<String> parsedCmd = Misc.ParseCommandLine(input);

            foreach (Command cmd in this._commands)
            {
                if (cmd.CommandName == parsedCmd[0])
                {
                    if(cmd.RequiredAccessLevel.Value <= WinttOS.UsersManager.CurrentUser.UserAccess.Value)
                    {
                        try
                        {
                            string result = cmd.Execute(parsedCmd.ToArray());
                            WinttCallStack.RegisterReturn();
                            return result;
                        }
                        catch (Exception e)
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
            return "Command '" + parsedCmd[0] + "' not exist, please type man <command> or help or more details.";
        }

        public string ProcessInput(ref TempUser user, string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.ProcessInput()",
                "string(ref TempUser, string)", "WinttOS.cs", 121));

            List<string> parsedCmd = Misc.ParseCommandLine(input);

            foreach (Command cmd in this._commands)
            {
                if (cmd.CommandName == parsedCmd[0])
                {
                    if (cmd.RequiredAccessLevel.Value <= user.UserAccess.Value)
                    {
                        user = null;
                        try
                        {
                            string result = cmd.Execute(parsedCmd.ToArray());
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
            return "Command '" + parsedCmd[0] + "' not exist, please type man <command> or help or more details.";
        }

        public List<String> GetCommandsList()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.GetCommandsList()",
                "List<string>()", "WinttOS.cs", 167));
            List<String> commands = new List<string>();
            foreach(Command command in this._commands)
            {
                if(!command.IsHiddenCommand)
                    commands.Add(command.CommandName);
            }
            WinttCallStack.RegisterReturn();
            return commands;
        }

        public List<Command> GetCommandsListInstances() => 
            _commands;

        public override void OnServiceTick()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.ServiceTick()",
                "void()", "WinttOS.cs", 184));
            try
            {
                if (Kernel.IsFinishingKernel)
                {
                    WinttCallStack.RegisterReturn();
                    return;
                }
                if (_didRunCycle)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(@$"{WinttOS.UsersManager.CurrentUser.Name}$0:\{GlobalData.CurrentDirectory}> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    _didRunCycle = false;
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
                    _didRunCycle = true;
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
