using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public sealed class HelpCommand : Command
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
            if (arguments[0] == "--alias" || arguments[0] == "-a")
            {
                return ExecuteHelp(true);
            }
            else
            {
                return ExecuteHelp(false);
            }
        }
        
        public IEnumerator<CoroutineControlPoint> PrintHelpStrAsync(bool showAliases)
        {
            int idx = 0;

            var commandsList = WinttOS.CommandManager.GetCommandsListInstances();

            for (; idx < 22 && idx < commandsList.Count; idx++)
            {
                if (showAliases)
                {
                    foreach (var value in commandsList[idx].CommandValues)
                    {
                        if (idx != commandsList[idx].CommandValues.Length - 1)
                            Console.Write(value + ", ");
                        else
                            Console.Write(value);
                    }
                }
                else
                {
                    Console.Write(commandsList[idx].CommandValues[0]);
                }
                Console.WriteLine(" (" + commandsList[idx].Description + ")");
            }

            if (idx >= commandsList.Count)
                yield break;

            Console.Write($"Press Space-bar to continue list ({idx + 1}/{commandsList.Count})...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Enter)
                        continue;
                    if (idx >= commandsList.Count)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You can see more information about a specific command by typing: {command} /help");
                        WinttOS.CommandManager.IsInputTaken = false;
                        break;
                    }

                    ShellUtils.ClearCurrentConsoleLine();
                    //ShellUtils.MoveCursorUp(1);
                    WinttDebugger.Debug($"Index: {idx}; List count: {commandsList.Count})", this);
                    if (showAliases)
                    {
                        foreach (var value in commandsList[idx].CommandValues)
                        {
                            if (idx != commandsList[idx].CommandValues.Length - 1)
                                Console.Write(value + ", ");
                            else
                                Console.Write(value);
                        }
                    }
                    else
                    {
                        Console.Write(commandsList[idx].CommandValues[0]);
                    }
                    Console.WriteLine(" (" + commandsList[idx++].Description + ")");

                    if (idx < commandsList.Count)
                        Console.Write($"Press Enter to continue list ({idx + 1}/{commandsList.Count})");
                }
                yield return null;
            }
        } 
        


        private ReturnInfo ExecuteHelp(bool showaliases)
        {
            CoroutinePool.Main.AddCoroutine(new(PrintHelpStrAsync(showaliases)));
            WinttOS.CommandManager.IsInputTaken = true;
            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Available command:");
            Console.WriteLine("- help /alias    show command aliases.");
        }

    }
}
