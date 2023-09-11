using Cosmos.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Programs;
using WinttOS.Base.Utils.Debugging;

namespace WinttOS.Base.Utils
{
    public class ShellUtils
    {
        private static List<string> recentInput = new();
        private static string currentInput = "";
        private static string inputToDisplay = "";
        private static int currentRecentPos = 0;
        private static ShellUtils instance => new ShellUtils();
        public static void ClearCurrentConsoleLine(int startPos = 0)
        {
            int currLineCursor = Console.CursorTop;
            Console.SetCursorPosition(startPos, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth - startPos));
            //Console.Write("\r" + new string(' ', Console.WindowWidth - 1 - startPos) + "\r");
            Console.SetCursorPosition(startPos, currLineCursor);
        }

        public static void MoveCursorUp(int steps = 1) => Console.SetCursorPosition(0, Console.CursorTop - steps);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isSuccessful">0 - OK; 1 - FAILED; 2 - WORKING; 3 - WARN</param>
        public static void PrintTaskResult(string task, ShellTaskResult isSuccessful, string detailes = "")
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            Console.Write("[");
            if (isSuccessful == ShellTaskResult.OK)  // I wanted to make it using swich case, but I'm too lazy to rewrite it 2 times :)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  OK  ");
            }
            else if (isSuccessful == ShellTaskResult.FAILED)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
            }
            else if (isSuccessful == ShellTaskResult.DOING)
                Console.Write(new string(' ', 6));
            else if (isSuccessful == ShellTaskResult.WARN)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" WARN ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"] {task} - {detailes}\n");
                return;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"] {task}\n");
        }

        [Obsolete("This method contains not working code! Please use Console.Readline()!", true)]
        public static string ProcessExtendedShellInput()
        {
            inputToDisplay = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.Enter)
                {
                    recentInput.Add(inputToDisplay);
                    if (recentInput.Count > 10)
                        recentInput.RemoveAt(0);
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    Console.WriteLine(inputToDisplay);
                    return inputToDisplay;
                }
                else if(key.Key == ConsoleKey.Backspace)
                {
                    if(inputToDisplay.Length > 0)
                        inputToDisplay = inputToDisplay.Substring(0, inputToDisplay.Length - 1);
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    Console.Write(inputToDisplay);
                }
                else if(key.Key == ConsoleKey.UpArrow)
                {
                    if(currentRecentPos < recentInput.Count - 1)
                    {
                        if (string.IsNullOrEmpty(currentInput))
                            currentInput = inputToDisplay;
                        currentRecentPos++;
                        WinttDebugger.Trace(currentRecentPos.ToString(), instance);
                        inputToDisplay = recentInput[currentRecentPos];
                        ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                        Console.Write(inputToDisplay);

                    }
                }
                else if(key.Key == ConsoleKey.DownArrow)
                {
                    if(currentRecentPos > 0)
                    {
                        currentRecentPos--;
                        WinttDebugger.Trace(currentRecentPos.ToString(), instance);
                        if (currentRecentPos == 0)
                        {
                            inputToDisplay = currentInput;
                            currentInput = "";
                        }
                        else
                        {
                            inputToDisplay = recentInput[currentRecentPos];
                            ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                            Console.Write(inputToDisplay);
                        }    
                    }
                }
                else if (!MIV.isForbiddenKey(key.Key))
                {
                    inputToDisplay += key.KeyChar;
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    Console.Write(inputToDisplay);
                }
            }
        }
    }

    public enum ShellTaskResult
    {
        OK,
        FAILED,
        DOING,
        WARN
    }
}
