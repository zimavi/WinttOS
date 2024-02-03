﻿using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;
using Cosmos.System.Coroutines;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class HelpCommand : Command
    {
        public HelpCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {

        }

        public override ReturnInfo Execute()
        {
            return ExecuteHelp(false);
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "/alias")
            {
                return ExecuteHelp(true);
            }
            else
            {
                return ExecuteHelp(false);
            }
        }
        /*
        public IEnumerator<CoroutineControlPoint> PrintHelpStrAsync()
        {
            List<string> helpStrs = HelpCommandManager.GetCommandsUsageStringsAsList();
            int index = 0;

            for (; index < 22 && index < helpStrs.Count; index++)
            {
                Console.WriteLine(helpStrs[index]);
            }

            if (index >= helpStrs.Count)
                yield break;

            Console.Write($"Press Space-bar to continue list ({index + 1}/{helpStrs.Count})...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Enter)
                        continue;
                    if (index >= helpStrs.Count)
                        yield break;

                    ShellUtils.ClearCurrentConsoleLine();
                    ShellUtils.MoveCursorUp(1);
                    WinttDebugger.Debug($"Index: {index}; List count: {helpStrs.Count})", this);
                    Console.WriteLine(helpStrs[index++]);

                    if (index < helpStrs.Count)
                        Console.Write($"Press Enter to continue list ({index + 1}/{helpStrs.Count})");
                }
                yield return null;
            }
        } 
        */


        private ReturnInfo ExecuteHelp(bool showaliases)
        {
            int count = 0;
            foreach (var command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                Console.Write("- ");
                if (showaliases)
                {
                    for (int i = 0; i < command.CommandValues.Length; i++)
                    {
                        if (i != command.CommandValues.Length - 1)
                        {
                            Console.Write(command.CommandValues[i] + ", ");
                        }
                        else
                        {
                            Console.Write(command.CommandValues[i]);
                        }
                    }
                }
                else
                {
                    Console.Write(command.CommandValues[0]);
                }
                Console.WriteLine(" (" + command.Description + ")");

                count++;
            }
            Console.WriteLine();
            Console.WriteLine("You can see more information about a specific command by typing: {command} /help");
            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Available command:");
            Console.WriteLine("- help /alias    show command aliases.");
        }

    }
}