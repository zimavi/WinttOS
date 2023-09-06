using Cosmos.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils
{
    public class ShellUtils
    {
        public static void ClearCurrentConsoleLine()
        {
            int currLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            //Console.Write(new string(' ', Console.WindowWidth));
            Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
            Console.SetCursorPosition(0, currLineCursor);
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
            Console.Write($"[");
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
            Console.WriteLine("] {task}\n");
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
