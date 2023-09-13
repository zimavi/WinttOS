using Cosmos.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinttOS.Core.Programs;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.Core.Utils
{
    public class ShellUtils
    {
        #region Variables

        private static List<string> recentInput = new();
        private static string currentInput = "";
        private static string inputToDisplay = "";
        private static int currentRecentPos = 0;
        private static ShellUtils instance => new();

        #endregion

        #region Static Methods
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

        //[Obsolete("This method contains not working code! Please use Console.Readline()!", true)]
        public static bool ProcessExtendedShellInput(ref string input)
        {
            if(Console.KeyAvailable)
            { 
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.Enter)
                {
                    recentInput.Insert(0, inputToDisplay);
                    if (recentInput.Count > 10)
                        recentInput.RemoveAt(recentInput.Count - 1);
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    Console.WriteLine(inputToDisplay);
                    input = inputToDisplay;
                    inputToDisplay = "";
                    currentInput = null;
                    return true;
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
                        if (currentInput == null)
                            currentInput = inputToDisplay;
                        if (currentRecentPos > 0)
                            currentRecentPos++;
                        if (currentRecentPos < 0) currentRecentPos = 1;
                        inputToDisplay = recentInput[currentRecentPos];
                        if (currentRecentPos == 0)
                            currentRecentPos++;
                        ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                        Console.Write(inputToDisplay);

                    }
                }
                else if(key.Key == ConsoleKey.DownArrow)
                {
                    if(currentRecentPos >= 0)
                    {
                        currentRecentPos--;
                        WinttDebugger.Trace(currentRecentPos.ToString(), instance);
                        if (currentRecentPos == -1)
                        {
                            inputToDisplay = currentInput;
                            currentInput = "";
                            ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                            Console.Write(inputToDisplay);
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

                    if (Console.CursorTop >= 26)
                        MoveCursorUp();
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    Console.Write(inputToDisplay);
                }
            }
            return false;
        }

        #endregion

        
        
    }

    public enum ShellTaskResult
    {
        OK,
        FAILED,
        DOING,
        WARN
    }
}
