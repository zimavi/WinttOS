using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Sys;
using WinttOS.Core;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.Shell.Commands.FileSystem;
using WinttOS.wSystem.Shell.Commands.Misc;
using WinttOS.wSystem.Shell.Commands.Screen;
using WinttOS.wSystem.Shell.Programs.RunCommands;
using WinttOS.wSystem.Shell.Commands.Processing;
using WinttOS.wSystem.Services;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Shell.Commands.Networking;
using WinttOS.wSystem.Shell.Commands.Users;
using WinttOS.wSystem.Shell.commands.Networking;
using System.Linq;
using Cosmos.System.Network;
using System.Windows.Input;
using System.IO;
using WinttOS.wSystem.Shell.bash;

namespace WinttOS.wSystem.Shell
{

    public class CommandManager : Service
    {
        private List<Command> _commands;
        private bool _didRunCycle = true;
        private bool _redirect = false;
        private string _commandOutput = "";


        public CommandManager() : base("WoshDaemon", "WoshManagerDaemon")
        {
            _commands = new List<Command>
            {
                new ClearScreenCommand(new string[] { "clear", "cls" }),
                new EchoCommand(new string[] { "echo" }),
                new VerisonCommand(new string[] { "version", "ver" }),
                new ShutdownCommand(new string[] { "shutdown", "power" }),
                new IpConfigCommand(new string[] { "ipconfig" }),
                new MakeFileCommand(new string[] { "mkfile" }),
                new MakeDirCommand(new string[] { "mkdir" }),
                new RmCommand(new string[] { "rm" }),
                new ChangeDirectoryCommand(new string[] { "cd" }),
                new DirCommand(new string[] { "dir", "ls" }),
                new HelpCommand(new string[] { "help" }),
                new ManCommand(new string[] { "man" }),
                new TimeCommand(new string[] { "time" }),
                new TouchCommand(new string[] { "touch" }),
                new SudoCommand(new string[] { "sudo" }),
                new WhoAmICommand(new string[] { "whoami" }),
                new MivCommand(new string[] { "miv" }),
                new CatUtilCommand(new string[] { "cat" }),
                new SystemCtlCommand(new string[] { "systemctl" }),
                new UsersCommand(new string[] { "user" }),
                new ProcessCommand(new string[] { "process" }),
                new WgetCommand(new string[] { "wget" }),
                new Package(new string[] { "apt-get", "apt" }),
                new PackageRepository(new string [] { "apt-get-repository", "apt-get-repo"})
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

        public void ProcessInput(string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.ProcessInput()",
                "string(string)", "WinttOS.cs", 77));

            if (input.Length <= 0)
            {
                Console.WriteLine();
                return;
            }

            #region Parse command

            string[] parts = input.Split(new char[] { '>' }, 2);
            string redirectionPart = parts.Length > 1 ? parts[1].Trim() : null;

            input = parts[0].Trim();

            if (!string.IsNullOrEmpty(redirectionPart))
            {
                _redirect = true;
                _commandOutput = "";
            }

            List<string> arguments = Misc.ParseCommandLine(input);

            string firstArg = arguments[0];

            if (arguments.Count > 0)
            {
                arguments.RemoveAt(0);
            }

            #endregion

            foreach (Command cmd in _commands)
            {
                if (cmd.ContainsCommand(firstArg))
                {
                    ReturnInfo result;

                    if (cmd.RequiredAccessLevel.Value > WinttOS.UsersManager.CurrentUser.UserAccess.Value)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("You do not have permission to run this command!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }

                    if (arguments.Count > 0 && (arguments[0] == "--help" || arguments[0] == "-h"))
                    {
                        showHelp(cmd);
                        result = new ReturnInfo(cmd, ReturnCode.OK);
                    }
                    else
                    {
                        result = CheckCommand(cmd);

                        if (result.Code == ReturnCode.OK)
                        {
                            if (arguments.Count == 0)
                            {
                                result = cmd.Execute();
                            }
                            else
                            {
                                result = cmd.Execute(arguments);
                            }
                        }
                    }

                    ProcessCommandResult(result);

                    if (_redirect)
                    {
                        _redirect = false;

                        Console.WriteLine();

                        HandleRedirection(redirectionPart, _commandOutput);

                        _commandOutput = "";
                    }

                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Unknown command.");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            if (_redirect)
            {
                _redirect = false;

                HandleRedirection(redirectionPart, _commandOutput);

                _commandOutput = "";
            }
        }

        public void ProcessInput(ref TempUser user, string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell.CommandManager.ProcessInput()",
                "string(string)", "WinttOS.cs", 77));

            if (input.Length <= 0)
            {
                Console.WriteLine();
                return;
            }

            #region Parse command

            string[] parts = input.Split(new char[] { '>' }, 2);
            string redirectionPart = parts.Length > 1 ? parts[1].Trim() : null;

            input = parts[0].Trim();

            if (!string.IsNullOrEmpty(redirectionPart))
            {
                _redirect = true;
                _commandOutput = "";
            }

            List<string> arguments = Misc.ParseCommandLine(input);

            string firstArg = arguments[0];

            if (arguments.Count > 0)
            {
                arguments.RemoveAt(0);
            }

            #endregion

            foreach (Command cmd in _commands)
            {
                if (cmd.ContainsCommand(firstArg))
                {
                    ReturnInfo result;

                    if (cmd.RequiredAccessLevel.Value > user.UserAccess.Value)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("You do not have permission to run this command!");
                        Console.ForegroundColor = ConsoleColor.White;
                        user = null;
                        return;
                    }

                    if (arguments.Count > 0 && (arguments[0] == "--help" || arguments[0] == "-h"))
                    {
                        showHelp(cmd);
                        result = new ReturnInfo(cmd, ReturnCode.OK);
                    }
                    else
                    {
                        result = CheckCommand(cmd);

                        if (result.Code == ReturnCode.OK)
                        {
                            if (arguments.Count == 0)
                            {
                                result = cmd.Execute();
                            }
                            else
                            {
                                result = cmd.Execute(arguments);
                            }
                        }
                    }

                    ProcessCommandResult(result);

                    if (_redirect)
                    {
                        _redirect = false;

                        Console.WriteLine();

                        HandleRedirection(redirectionPart, _commandOutput);

                        _commandOutput = "";
                    }
                    user = null;

                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Unknown command.");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            if (_redirect)
            {
                _redirect = false;

                HandleRedirection(redirectionPart, _commandOutput);

                _commandOutput = "";
            }
        }

        private void showHelp(Command cmd)
        {
            Console.WriteLine("Description: " + cmd.Description + '.');
            Console.WriteLine();
            if (cmd.CommandValues.Length > 1)
            {
                Console.Write("Aliases: ");
                for (int i = 0; i < cmd.CommandValues.Length; i++)
                {
                    if (i != cmd.CommandValues.Length - 1)
                    {
                        Console.Write(cmd.CommandValues[i] + ", ");
                    }
                    else
                    {
                        Console.Write(cmd.CommandValues[i]);
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            cmd.PrintHelp();
        }

        private ReturnInfo CheckCommand(Command command)
        {
            if (command.Type == CommandType.Filesystem)
            {
                if (GlobalData.FileSystem == null || GlobalData.FileSystem.GetVolumes().Count == 0)
                {
                    return new ReturnInfo(command, ReturnCode.ERROR, "No volume detected!");
                }
            }
            if (command.Type == CommandType.Network)
            {
                if (NetworkStack.ConfigEmpty())
                {
                    return new ReturnInfo(command, ReturnCode.ERROR, "No network configuration detected! Use ipconfig /set.");
                }
            }
            return new ReturnInfo(command, ReturnCode.OK);
        }

        private void ProcessCommandResult(ReturnInfo result)
        {
            if (result.Code == ReturnCode.ERROR_ARG)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Command arguments are incorrectly formatted.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (result.Code == ReturnCode.ERROR)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: " + result.Info);
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();
        }

        private void HandleRedirection(string filePath, string commandOutput)
        {
            string fullPath = GlobalData.CurrentDirectory + filePath;

            File.WriteAllText(fullPath, commandOutput);
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
                    Console.Write(@$"{WinttOS.UsersManager.CurrentUser.Name}${GlobalData.CurrentDirectory}> ");
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
                    ProcessInput(input);
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
