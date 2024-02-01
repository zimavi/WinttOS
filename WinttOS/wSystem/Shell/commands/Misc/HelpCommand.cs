using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Shell.Utils.Commands;
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
            CoroutinePool.Main.AddCoroutine(new(PrintHelpStrAsync()));
            return new(this, ReturnCode.OK);
        }

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
    }
}
