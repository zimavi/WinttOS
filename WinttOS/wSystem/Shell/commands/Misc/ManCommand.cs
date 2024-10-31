using Cosmos.System;
using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class ManCommand : Command
    {
        public ManCommand(string[] name) : base(name, AccessLevel.Default)
        {
            CommandManual = new List<string>()
            {
                "NAME",
                "   man - Display the manual for commands",
                "",
                "SINOPSIS",
                "   man [COMMAND]",
                "",
                "DESCRIPTION",
                "   The man command displays the manual for the specified command, providing",
                "   detailed information about its usage, options, and functionality. If no",
                "   manual exists for the specified command, an error message will be displayed.",
                "",
                "OPTIONS",
                "",
                "   COMMAND",
                "       The name of the command for which you want to see the manual. If the",
                "       command has a manual, it will be printed to the terminal.",
                "",
                "EXAMPLES",
                "   Display the manual for a command:",
                "       man user",
                "",
                "NOTES",
                "    - The man command relies on the command's registered manuals.",
                "      Ensure that the command has an associated manual for proper usage.",
                "    - If the terminal height is less than the manual content, the",
                "      user will be prompted to scroll through the content.",
                "    - When in scrolling mode, use 'B' to scroll back and 'Q' to exit view.",
                "",
                "AUTHOR",
                "   zimavi"
            }
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                for(int i = 0; i < command.CommandValues.Length; i++)
                {
                    if (command.CommandValues[i] == arguments[0])
                    {
                        if (command.CommandManual != null && command.CommandManual.Count > 0)
                        {
                            WinttOS.CommandManager.IsInputTaken = true;
                            CoroutinePool.Main.AddCoroutine(new(PrintManualAsync(command.CommandManual)));
                            return new(this, ReturnCode.OK);
                        }
                        break;
                    }
                }
            }

            return new(this, ReturnCode.ERROR, $"There is no manual for {arguments[0]} command!");
        }

        private static IEnumerator<CoroutineControlPoint> PrintManualAsync(List<string> manual)
        {
            int idx = 0;
            int termHeight = 22;

            if(WinttOS.IsTty)
            {
                termHeight = WinttOS.Tty.Rows;
            }

            for(; idx < termHeight && idx < manual.Count; idx++)
            {
                SystemIO.STDOUT.PutLine(manual[idx]);
            }

            if(idx >= manual.Count - 1)
            {
                WinttOS.CommandManager.IsInputTaken = false;
                yield break;
            }

            SystemIO.STDOUT.Put($"Press Space-bar to continue list ({idx + 1}/{manual.Count})...");

            while(true) 
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Enter 
                        && key.Key != ConsoleKey.B && key.Key != ConsoleKey.Q)
                        continue;

                    if(key.Key == ConsoleKey.B)
                    {
                        if (WinttOS.IsTty)
                        {
                            WinttOS.Tty.ScrollUp();
                            idx--;
                            WinttOS.Tty.X--;
                            SystemIO.STDOUT.Put($"Press Space-bar to continue list ({idx + 1}/{manual.Count})...");
                        }
                        continue;
                    }
                    else if(key.Key == ConsoleKey.Q)
                    {
                        if (WinttOS.IsTty)
                        {
                            WinttOS.Tty.ClearText();
                        }
                        else
                        {
                            Console.Clear();
                        }
                        yield break;
                    }
                    if(idx >= manual.Count - 1)
                    {
                        WinttOS.CommandManager.IsInputTaken = false;
                        yield break;
                    }

                    ShellUtils.ClearCurrentConsoleLine();

                    SystemIO.STDOUT.PutLine(manual[idx]);

                    idx++;

                    if(idx < manual.Count - 1)
                        SystemIO.STDOUT.Put($"Press Space-bar to continue list ({idx + 1}/{manual.Count})...");
                }

                yield return null;
            }
        }

        public override ReturnInfo Execute()
        {
            List<string[]> commandsWithManuals = new List<string[]>();
            foreach (Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                if (command.CommandManual.Any())
                    commandsWithManuals.Add(command.CommandValues);

            }
            string returnStr = "List of commands with manuals:\n";

            List<string> formatted = new();

            foreach(var v in commandsWithManuals)
            {
                formatted.Add(string.Join(", ", v));
            }
            returnStr += string.Join('\n', formatted.ToArray());
            SystemIO.STDOUT.PutLine(returnStr);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("man [command name]");
        }
    }
}
