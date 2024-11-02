using Cosmos.System.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinttOS.Core;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Shell.bash;
using WinttOS.wSystem.Shell.commands.FileSystem;
using WinttOS.wSystem.Shell.commands.Misc;
using WinttOS.wSystem.Shell.commands.Networking;
using WinttOS.wSystem.Shell.commands.Processing;
using WinttOS.wSystem.Shell.Commands.FileSystem;
using WinttOS.wSystem.Shell.Commands.Misc;
using WinttOS.wSystem.Shell.Commands.Networking;
using WinttOS.wSystem.Shell.Commands.Processing;
using WinttOS.wSystem.Shell.Commands.Screen;
using WinttOS.wSystem.Shell.Commands.Users;
using WinttOS.wSystem.Shell.Programs.RunCommands;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell
{

    public sealed class CommandManager : Service
    {
        private List<Command> _commands;
        private bool _didRunCycle = true;
        private bool _redirect = false;
        internal string _commandOutput = "";
        private bool _hasShellFired = false;
        public bool IsInputTaken = false;
        public bool _executeNewCommand = false;

        public CommandManager() : base("Shelld", "shell.service")
        {
            _commands = new List<Command>
            {
                new ClearScreenCommand(new string[] { "clear", "cls" }),
                new EchoCommand(new string[] { "echo" }),
                new VerisonCommand(new string[] { "version", "ver" }),
                new ShutdownCommand(new string[] { "shutdown" }),
                new RebootCommand(new string[] { "reboot" }),
                new HaltCommand(new string[] { "halt" }),
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
                new MivCommand(new string[] { "miv" }),
                new CatUtilCommand(new string[] { "cat" }),
                new SystemCtlCommand(new string[] { "systemctl" }),
                new UsersCommand(new string[] { "user" }),
                new ProcessCommand(new string[] { "process" }),
                new WgetCommand(new string[] { "wget" }),
                new PackageCommand(new string[] { "packos", "pack", "pkg" }),
                new PackageRepository(new string [] { "packos-repo", "pack-repo", "pkg-repo"}),
                new DnsCommand(new string[] { "dns" }),
                new PingCommand(new string[] { "ping" }),
                new EnvironmentCommand(new string[] { "export" }),
                new RunCommand(new string[] {"run"}),
                new HashCommand(new string[] { "hash" }),
                new SystemInfoCommand(new string[] { "sysinfo" }),
                new DisksCommand(new string[] { "disks" }),
                new LogsCommand(new string[] { "logs" }),
                new TreeCommand(new string[] { "tree" }),
                new PlayBadAppleCommand(new string[] { "bad_apple" }),

                new CommandAction(new string[] { "whoami" }, AccessLevel.Default, () =>
                {
                    SystemIO.STDOUT.PutLine(UsersManager.userLogged);
                }),
                new CommandAction(new string[] { "bash" }, AccessLevel.Default, () =>
                {
                    BashInterpreter bash = new();
                    SystemIO.STDOUT.PutLine(bash.Parse(@"0:\startup.sh"));
                    bash.Execute();
                }),
                new CommandAction(new string[] { "crash" }, AccessLevel.Administrator, () =>
                {
                    Kernel.WinttRaiseHardError("Crash command was executed", this);
                }),
                new CommandAction(new string[] { "memory", "mem" }, AccessLevel.Default, () =>
                {
                    SystemIO.STDOUT.PutLine($"Available memory: " + Filesystem.Utils.ConvertSize(WinttOS.MemoryManager.FreeMemory * 1024 * 1024));
                    SystemIO.STDOUT.PutLine($"Used memory: " + Filesystem.Utils.ConvertSize(Memory.GetUsedMemory() * 1024 * 1024));
                    SystemIO.STDOUT.PutLine($"Used memory (%): {100 - WinttOS.MemoryManager.FreePercentage}%");
                    SystemIO.STDOUT.PutLine($"Total memory: " + Filesystem.Utils.ConvertSize(Memory.TotalMemory * 1024 * 1024));
                })
            };
        }

        /// <summary>
        /// Adds command to command list
        /// </summary>
        /// <param name="command">Command's class that implements <see cref="Command"/> abstract class</param>
        /// <returns>true if successful</returns>
        public bool RegisterCommand(Command command)
        {
            try
            {
                this._commands.Add(command);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ProcessInput(string input)
        {
            if (input.Length <= 0)
            {
                SystemIO.STDOUT.PutLine("");
                return;
            }

            #region Parse command

            string[] parts = input.Split(new char[] { '>' }, 2);
            string redirectionPart = parts.Length > 1 ? parts[1].Trim() : null;


            input = parts[0].Trim();

            if (!string.IsNullOrEmpty(redirectionPart))
            {
                _redirect = true;
            }
            _commandOutput = "";

            parts = input.Split("&&");
            if (parts.Length > 1)
            {
                _executeNewCommand = true;
            }

            int i = 0;

            executeNew:

            List<string> arguments = Misc.ParseCommandLine(parts[i]);

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

                    if (cmd.RequiredAccessLevel.Value > UsersManager.LoggedLevel.Value)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        SystemIO.STDOUT.PutLine("You do not have permission to run this command!");
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
                            _hasShellFired = false;
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

                        SystemIO.STDOUT.PutLine("");

                        HandleRedirection(redirectionPart, _commandOutput);

                        _commandOutput = "";
                    }

                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            SystemIO.STDOUT.PutLine("Unknown command.");
            Console.ForegroundColor = ConsoleColor.White;

            SystemIO.STDOUT.PutLine("");

            if (_redirect)
            {
                _redirect = false;

                HandleRedirection(redirectionPart, _commandOutput);

                _commandOutput = "";
            }

            if (_executeNewCommand)
            {
                i++;
                if (parts.Length == i)
                    goto executionEnd;
                goto executeNew;
            }

            executionEnd:;
        }

        private void showHelp(Command cmd)
        {
            SystemIO.STDOUT.PutLine("Description: " + cmd.Description + '.');
            SystemIO.STDOUT.PutLine("");
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
                SystemIO.STDOUT.PutLine("");
                SystemIO.STDOUT.PutLine("");
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
                SystemIO.STDOUT.PutLine("Command arguments are incorrectly formatted.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (result.Code == ReturnCode.ERROR)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                SystemIO.STDOUT.PutLine("Error: " + result.Info);
                Console.ForegroundColor = ConsoleColor.White;
            }

            SystemIO.STDOUT.PutLine("");
        }

        private void HandleRedirection(string filePath, string commandOutput)
        {
            foreach(var c in _commands)
            {
                if(c.CommandValues.Contains(filePath))
                {
                    ProcessInput(filePath + " " + commandOutput);
                    return;
                }
            }

            string fullPath = GlobalData.CurrentDirectory + filePath;

            File.WriteAllText(fullPath, commandOutput);
        }

        public List<Command> GetCommandsListInstances() => 
            _commands;

        public override void OnServiceTick()
        {
            try
            {
                if (Kernel.IsFinishingKernel)
                {
                    return;
                }
                if (WinttOS.IsTty)
                {
                    if (_didRunCycle && !IsInputTaken)
                    {
                        WinttOS.Tty.Foreground = ConsoleColor.DarkGray;
                        WinttOS.Tty.Write(@$"{UsersManager.userLogged}${GlobalData.CurrentDirectory}> ");
                        WinttOS.Tty.Foreground = ConsoleColor.White;
                        _didRunCycle = false;
                        _hasShellFired = true;
                    }
                    else if (!IsInputTaken && !_hasShellFired)
                    {
                        WinttOS.Tty.Foreground = ConsoleColor.DarkGray;
                        WinttOS.Tty.Write(@$"{UsersManager.userLogged}${GlobalData.CurrentDirectory}> ");
                        WinttOS.Tty.Foreground = ConsoleColor.White;
                        _didRunCycle = false;
                        _hasShellFired = true;
                    }
                    string input = "";
                    bool hasKey = ShellUtils.ProcessExtendedInput(ref input);
                    if (hasKey)
                    {
                        PowerManagerService.isIdling = false;
                        string[] split = input.Split(' ');
                        WinttOS.Tty.Foreground = ConsoleColor.Gray;
                        ProcessInput(input);
                        _didRunCycle = true;
                    }
                    else
                        PowerManagerService.isIdling = true;
                }
                else
                {
                    if (_didRunCycle && !IsInputTaken)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(@$"{UsersManager.userLogged}${GlobalData.CurrentDirectory}> ");
                        Console.ForegroundColor = ConsoleColor.White;
                        _didRunCycle = false;
                        _hasShellFired = true;
                    }
                    else if (!IsInputTaken && !_hasShellFired)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(@$"{UsersManager.userLogged}${GlobalData.CurrentDirectory}> ");
                        Console.ForegroundColor = ConsoleColor.White;
                        _didRunCycle = false;
                        _hasShellFired = true;
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
