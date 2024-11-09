using Cosmos.Core;
using Cosmos.Core.Memory;
using System;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.Programs
{
    public sealed class MIV
    {
        private static int _width;
        private static int _height;
        public static void printMIVStartScreen()
        {
            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();

            string[] lines = {
                "MIV - Minimalistic Vi",
                "",
                "version 1.3",
                "by Denis Bartashevich and zimavi",
                "Minor additions by CaveSponge",
                "MIV is open source and freely distributable",
                "",
                "type :help<Enter>          for information",
                "type :q<Enter>                     to exit",
                "type :wq<Enter       save to file and exit",
                "press i                           to write"
            };

            PrintCentered(lines);
        }

        public static void PrintCentered(string[] lines)
        {

            int startingRow = (_height / 2) - (lines.Length / 2);

            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int padding = (_width - lines[i].Length) / 2;
                if(WinttOS.IsTty)
                {
                    WinttOS.Tty.X = padding;
                    WinttOS.Tty.Y = startingRow + i;
                    WinttOS.Tty.WriteNoUpdate(line + '\n');
                }
                else
                {
                    Console.SetCursorPosition(padding, startingRow + i);
                    Console.WriteLine(line);
                }
            }

            if (WinttOS.IsTty)
                WinttOS.Tty.Update();
        }
        public static string stringCopy(string value)
        {
            string newString = string.Empty;

            for (int i = 0; i < value.Length - 1; i++)
            {
                newString += value[i];
            }

            return newString;
        }

        public static void printMIVScreen(char[] chars, int pos, string infoBar, Boolean editMode)
        {
            int countNewLine = 0;
            int countChars = 0;
            //delay(10000000);
            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();

            for (int i = 0; i < pos; i++)
            {
                if (chars[i] == '\n')
                {
                    if (WinttOS.IsTty)
                        WinttOS.Tty.WriteNoUpdate("\n");
                    else
                        Console.WriteLine();
                    countNewLine++;
                    countChars = 0;
                }
                else
                {
                    if (WinttOS.IsTty)
                        WinttOS.Tty.WriteNoUpdate(chars[i]);
                    else
                        Console.Write(chars[i]);
                    countChars++;
                    if (countChars % _width == _width - 1)
                    {
                        if (WinttOS.IsTty)
                            WinttOS.Tty.WriteNoUpdate("\n");
                        else
                            Console.WriteLine();
                        countNewLine++;
                        countChars = 0;
                    }
                }
            }

            if (WinttOS.IsTty)
                WinttOS.Tty.WriteNoUpdate("/");
            else
                Console.Write("/");

            for (int i = 0; i < _height - 2 - countNewLine; i++)
            {
                if (WinttOS.IsTty)
                {
                    WinttOS.Tty.WriteNoUpdate("\n");
                    WinttOS.Tty.WriteNoUpdate("~");
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("~");
                }
            }

            //PRINT INSTRUCTION
            SystemIO.STDOUT.PutLine("");
            for (int i = 0; i < _width - 8; i++)
            {
                if (i < infoBar.Length)
                {
                    if (WinttOS.IsTty)
                        WinttOS.Tty.WriteNoUpdate(infoBar[i]);
                    else
                        Console.Write(infoBar[i]);
                }
                else
                {
                    if (WinttOS.IsTty)
                        WinttOS.Tty.WriteNoUpdate(" ");
                    else
                        Console.Write(" ");
                }
            }

            if (editMode)
            {
                if (WinttOS.IsTty)
                    WinttOS.Tty.WriteNoUpdate(countNewLine + 1 + "," + countChars);
                else
                    Console.WriteLine();
            }

            if (WinttOS.IsTty)
                WinttOS.Tty.Update();

        }

        public static string miv(string start)
        {
            Boolean editMode = false;
            int pos = 0;
            char[] chars = new char[2000];
            string infoBar = string.Empty;

            if (start == null)
            {
                printMIVStartScreen();
            }
            else
            {
                pos = start.Length;

                for (int i = 0; i < start.Length; i++)
                {
                    chars[i] = start[i];
                }
                printMIVScreen(chars, pos, infoBar, editMode);
            }

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = SystemIO.STDIN.GetChr(true);

                if (isForbiddenKey(keyInfo.Key)) continue;

                else if (!editMode && keyInfo.KeyChar == ':')
                {
                    infoBar = ":";
                    printMIVScreen(chars, pos, infoBar, editMode);
                    do
                    {
                        keyInfo = SystemIO.STDIN.GetChr(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            if (infoBar == ":wq")
                            {
                                string returnString = string.Empty;
                                for (int i = 0; i < pos; i++)
                                {
                                    returnString += chars[i];
                                }
                                return returnString;
                            }
                            else if (infoBar == ":q")
                            {
                                return null;

                            }
                            else if (infoBar == ":help")
                            {
                                printMIVStartScreen();
                                break;
                            }
                            else
                            {
                                infoBar = "ERROR: No such command";
                                printMIVScreen(chars, pos, infoBar, editMode);
                                break;
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.Backspace)
                        {
                            infoBar = stringCopy(infoBar);
                            printMIVScreen(chars, pos, infoBar, editMode);
                        }
                        else if (keyInfo.KeyChar == 'q')
                        {
                            infoBar += "q";
                        }
                        else if (keyInfo.KeyChar == ':')
                        {
                            infoBar += ":";
                        }
                        else if (keyInfo.KeyChar == 'w')
                        {
                            infoBar += "w";
                        }
                        else if (keyInfo.KeyChar == 'h')
                        {
                            infoBar += "h";
                        }
                        else if (keyInfo.KeyChar == 'e')
                        {
                            infoBar += "e";
                        }
                        else if (keyInfo.KeyChar == 'l')
                        {
                            infoBar += "l";
                        }
                        else if (keyInfo.KeyChar == 'p')
                        {
                            infoBar += "p";
                        }
                        else
                        {
                            continue;
                        }
                        printMIVScreen(chars, pos, infoBar, editMode);



                    } while (keyInfo.Key != ConsoleKey.Escape);
                }

                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    editMode = false;
                    infoBar = string.Empty;
                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }

                else if (keyInfo.Key == ConsoleKey.I && !editMode)
                {
                    editMode = true;
                    infoBar = "-- INSERT --";
                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }

                else if (keyInfo.Key == ConsoleKey.Enter && editMode && pos >= 0)
                {
                    chars[pos++] = '\n';
                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && editMode && pos >= 0)
                {
                    if (pos > 0) pos--;

                    chars[pos] = '\0';

                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }

                if (editMode && pos >= 0)
                {
                    chars[pos++] = keyInfo.KeyChar;
                    printMIVScreen(chars, pos, infoBar, editMode);
                }

                Heap.Collect();

            } while (true);
        }

        public static bool isForbiddenKey(ConsoleKey key)
        {
            ConsoleKey[] forbiddenKeys = { ConsoleKey.Print, ConsoleKey.PrintScreen, ConsoleKey.Pause, ConsoleKey.Home, ConsoleKey.PageUp, ConsoleKey.PageDown, ConsoleKey.End, ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3, ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7, ConsoleKey.NumPad8, ConsoleKey.NumPad9, ConsoleKey.Insert, ConsoleKey.F1, ConsoleKey.F2, ConsoleKey.F3, ConsoleKey.F4, ConsoleKey.F5, ConsoleKey.F6, ConsoleKey.F7, ConsoleKey.F8, ConsoleKey.F9, ConsoleKey.F10, ConsoleKey.F11, ConsoleKey.F12, ConsoleKey.Add, ConsoleKey.Divide, ConsoleKey.Multiply, ConsoleKey.Subtract, ConsoleKey.LeftWindows, ConsoleKey.RightWindows };
            for (int i = 0; i < forbiddenKeys.Length; i++)
            {
                if (key == forbiddenKeys[i]) return true;
            }
            return false;
        }

        public static void delay(int time)
        {
            for (int i = 0; i < time; i++) ;
        }
        public static void StartMIV(string path)
        {
            _width = WinttOS.IsTty ? WinttOS.Tty.Cols : 80;
            _height = WinttOS.IsTty ? WinttOS.Tty.Rows : 25;


            GlobalData.FileToEdit = path;

            string text = string.Empty;
            if (path == null)
            {
                text = miv(null);
            }
            else
            {
                if (File.Exists(GlobalData.CurrentDirectory + GlobalData.FileToEdit))
                {
                    text = miv(File.ReadAllText(GlobalData.CurrentDirectory + GlobalData.FileToEdit));
                }
                else
                {
                    text = miv(null);
                }
            }

            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();

            if (text != null)
            {
                File.WriteAllText(GlobalData.CurrentDirectory + GlobalData.FileToEdit, text);
                SystemIO.STDOUT.PutLine("Content has been saved to " + GlobalData.FileToEdit);
            }
        }
    }
}