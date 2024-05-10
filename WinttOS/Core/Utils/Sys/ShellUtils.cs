using System;
using System.Collections.Generic;
using System.Drawing;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.GUI;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Shell.Programs;

namespace WinttOS.Core.Utils.Sys
{
    using Sys = wSystem.WinttOS;
    public sealed class ShellUtils
    {
        #region Variables

        private static List<string> _recentInput = new();
        private static string _currentInput = "";
        private static string _inputToDisplay = "";
        private static int _currentRecentPos = 0;
        private static ShellUtils _instance => new();

        #endregion

        #region Static Methods

        public static void ClearCurrentConsoleLine(int startPos = 0)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Sys.ShellUtils.ClearCurrentConsoleLine()",
                "void(int)", "ShellUtils.cs", 28));
            if (Sys.IsTty)
            {
                int y = Sys.Tty.Y;
                Sys.Tty.X = startPos;
                Sys.Tty.Write(new string('\0', Sys.Tty.Cols - startPos - 1));
                Sys.Tty.X = startPos;
                Sys.Tty.Y = y;
            }
            else
            {
                int currLineCursor = Console.CursorTop;
                Console.SetCursorPosition(startPos, Console.CursorTop);
                Console.Write(new string('\0', Console.WindowWidth - startPos - 1));
                //Console.Write("\r" + new string(' ', Console.WindowWidth - 1 - startPos) + "\r");
                Console.SetCursorPosition(startPos, currLineCursor);
            }
            WinttCallStack.RegisterReturn();
        }

        public static void MoveCursorUp(int steps = 1)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Sys.ShellUtils.MoveCursorUp()",
                "void(int)", "ShellUtils.cs", 40));
            if (Sys.IsTty)
                Sys.Tty.Y = Sys.Tty.Y - 1;
            else
                Console.SetCursorPosition(0, Console.CursorTop - steps);
            WinttCallStack.RegisterReturn();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isSuccessful">0 - OK; 1 - FAILED; 2 - WORKING; 3 - WARN</param>
        public static void PrintTaskResult(string task, ShellTaskResult isSuccessful, string detailes = "")
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Sys.ShellUtils.PrintTaskResult",
                "void(string, ShellTaskResult, string)", "ShellUtils.cs", 53));
            if (!Sys.KernelPrint)
                return;
            if(Sys.IsTty)
            {
                ClearCurrentConsoleLine();
                Sys.Tty.Write("[");

                var originColor = Sys.Tty.ForegroundColor;
                
                switch(isSuccessful)
                {
                    case ShellTaskResult.OK:
                        Sys.Tty.ForegroundColor = Color.Green;
                        Sys.Tty.Write("  OK  ");
                        break;
                    case ShellTaskResult.FAILED:
                        Sys.Tty.ForegroundColor = Color.Red;
                        Sys.Tty.Write("FAILED");
                        break;
                    case ShellTaskResult.DOING:
                        Sys.Tty.ForegroundColor = Color.Red;
                        Sys.Tty.Write(" **** ");
                        break;
                    case ShellTaskResult.WARN:
                        Sys.Tty.ForegroundColor = Color.Yellow;
                        Sys.Tty.Write(" WARN ");
                        break;
                    case ShellTaskResult.NONE:
                        Sys.Tty.Write("      ");
                        break;
                }

                Sys.Tty.ForegroundColor = Color.White;

                Sys.Tty.Write("] ");

                Sys.Tty.ForegroundColor = Color.Gray;

                Sys.Tty.Write(task + ": ");

                Sys.Tty.ForegroundColor = Color.White;

                Sys.Tty.WriteLine(detailes);
            }
            else
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
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" **** ");
                }
                else if (isSuccessful == ShellTaskResult.WARN)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" WARN ");
                }
                else
                    Console.Write("      ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(task + ": ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(detailes);
                WinttCallStack.RegisterReturn();
            }
        }

        //[Obsolete("This method contains not working code! Please use Console.Readline()!", true)]
        public static bool ProcessExtendedInput(ref string input)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Sys.ShellUtils.ProcessExtendedInput()",
                "bool(ref string)", "ShellUtils.cs", 86));
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = SystemIO.STDIN.GetChr(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    _recentInput.Insert(0, _inputToDisplay);
                    if (_recentInput.Count > 10)
                        _recentInput.RemoveAt(_recentInput.Count - 1);
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    SystemIO.STDOUT.PutLine(_inputToDisplay);
                    input = _inputToDisplay;
                    _inputToDisplay = "";
                    _currentInput = null;
                    WinttCallStack.RegisterReturn();
                    return true;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (_inputToDisplay.Length > 0)
                        _inputToDisplay = _inputToDisplay.Substring(0, _inputToDisplay.Length - 1);
                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    SystemIO.STDOUT.Put(_inputToDisplay);
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if (_currentRecentPos < _recentInput.Count - 1)
                    {
                        if (_currentInput == null)
                            _currentInput = _inputToDisplay;
                        if (_currentRecentPos > 0)
                            _currentRecentPos++;
                        if (_currentRecentPos < 0) _currentRecentPos = 1;
                        _inputToDisplay = _recentInput[_currentRecentPos];
                        if (_currentRecentPos == 0)
                            _currentRecentPos++;
                        ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                        SystemIO.STDOUT.Put(_inputToDisplay);

                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (_currentRecentPos >= 0)
                    {
                        _currentRecentPos--;
                        WinttDebugger.Trace(_currentRecentPos.ToString(), _instance);
                        if (_currentRecentPos == -1)
                        {
                            _inputToDisplay = _currentInput;
                            _currentInput = "";
                            ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                            SystemIO.STDOUT.Put(_inputToDisplay);
                        }
                        else
                        {
                            _inputToDisplay = _recentInput[_currentRecentPos];
                            ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                            SystemIO.STDOUT.Put(_inputToDisplay);
                        }
                    }
                }
                else if (!MIV.isForbiddenKey(key.Key))
                {
                    _inputToDisplay += key.KeyChar;

                    ClearCurrentConsoleLine(GlobalData.ShellClearStartPos);
                    SystemIO.STDOUT.Put(_inputToDisplay);
                }
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        #endregion

        public static string ReadLineWithInterception()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Sys.ShellUtils.ReadLineWithInterception()",
                "string()", "ShellUtils.cs", 165));
            string input = "";
            ConsoleKey key;
            do
            {
                var keyInfo = SystemIO.STDIN.GetChr(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && input.Length > 0)
                {
                    SystemIO.STDOUT.Put("\b \b");
                    input = input[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    SystemIO.STDOUT.Put("*");
                    input += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            WinttCallStack.RegisterReturn();
            return input;
        }

    }

    public enum ShellTaskResult
    {
        NONE,
        OK,
        FAILED,
        DOING,
        WARN
    }
}
