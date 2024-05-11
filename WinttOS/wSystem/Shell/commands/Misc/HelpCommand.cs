using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
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

            int termHeight = 22;

            if (WinttOS.IsTty)
            {
                termHeight = WinttOS.Tty.Rows;
            }

            for (; idx < termHeight && idx < commandsList.Count; idx++)
            {
                if (showAliases)
                {
                    foreach (var value in commandsList[idx].CommandValues)
                    {
                        if (idx != commandsList[idx].CommandValues.Length - 1)
                            SystemIO.STDOUT.Put(value + ", ");
                        else
                            SystemIO.STDOUT.Put(value);

                        WinttDebugger.Debug($"Index: {idx}; List count: {commandsList.Count})", this);
                    }
                }
                else
                {
                    SystemIO.STDOUT.Put(commandsList[idx].CommandValues[0]);
                    WinttDebugger.Debug($"Index: {idx}; List count: {commandsList.Count})", this);
                }
                SystemIO.STDOUT.PutLine(" (" + commandsList[idx].Description + ")");
            }

            if (idx >= commandsList.Count - 1)
            {
                SystemIO.STDOUT.PutLine("");
                SystemIO.STDOUT.PutLine("You can see more information about a specific command by typing: {command} /help");
                WinttOS.CommandManager.IsInputTaken = false;
                yield break;
            }

            SystemIO.STDOUT.Put($"Press Space-bar to continue list ({idx + 1}/{commandsList.Count})...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Enter)
                        continue;
                    if (idx >= commandsList.Count - 1)
                    {
                        SystemIO.STDOUT.PutLine("");
                        SystemIO.STDOUT.PutLine("You can see more information about a specific command by typing: {command} /help");
                        WinttOS.CommandManager.IsInputTaken = false;
                        yield break;
                    }

                    ShellUtils.ClearCurrentConsoleLine();
                    //ShellUtils.MoveCursorUp(1);
                    WinttDebugger.Debug($"Index: {idx}; List count: {commandsList.Count})", this);
                    if (showAliases)
                    {
                        foreach (var value in commandsList[idx].CommandValues)
                        {
                            if (idx != commandsList[idx].CommandValues.Length - 1)
                                SystemIO.STDOUT.Put(value + ", ");
                            else
                                SystemIO.STDOUT.Put(value);
                        }
                    }
                    else
                    {
                        SystemIO.STDOUT.Put(commandsList[idx].CommandValues[0]);
                    }
                    SystemIO.STDOUT.PutLine(" (" + commandsList[idx].Description + ")");

                    idx++;

                    if (idx < commandsList.Count - 1)
                        SystemIO.STDOUT.Put($"Press Enter to continue list ({idx + 1}/{commandsList.Count})");
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
            SystemIO.STDOUT.PutLine("Available command:");
            SystemIO.STDOUT.PutLine("- help /alias    show command aliases.");
        }

    }
}
