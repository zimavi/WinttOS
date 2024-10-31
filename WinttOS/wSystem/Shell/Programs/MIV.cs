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
        public static void printMIVStartScreen()
        {
            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~                               MIV - MInimalistic Vi");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~                                  version 1.2");
            SystemIO.STDOUT.PutLine("~                             by Denis Bartashevich");
            SystemIO.STDOUT.PutLine("~                            Minor additions by CaveSponge");
            SystemIO.STDOUT.PutLine("~                    MIV is open source and freely distributable");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~                     type :help<Enter>          for information");
            SystemIO.STDOUT.PutLine("~                     type :q<Enter>             to exit");
            SystemIO.STDOUT.PutLine("~                     type :wq<Enter>            save to file and exit");
            SystemIO.STDOUT.PutLine("~                     press i                    to write");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.PutLine("~");
            SystemIO.STDOUT.Put("~");
        }
        public static String stringCopy(String value)
        {
            String newString = String.Empty;

            for (int i = 0; i < value.Length - 1; i++)
            {
                newString += value[i];
            }

            return newString;
        }

        public static void printMIVScreen(char[] chars, int pos, String infoBar, Boolean editMode)
        {
            int countNewLine = 0;
            int countChars = 0;
            delay(10000000);
            if (WinttOS.IsTty)
                WinttOS.Tty.ClearText();
            else
                Console.Clear();

            for (int i = 0; i < pos; i++)
            {
                if (chars[i] == '\n')
                {
                    SystemIO.STDOUT.PutLine("");
                    countNewLine++;
                    countChars = 0;
                }
                else
                {
                    SystemIO.STDOUT.Put(chars[i]);
                    countChars++;
                    if (countChars % 80 == 79)
                    {
                        countNewLine++;
                    }
                }
            }

            SystemIO.STDOUT.Put("/");

            for (int i = 0; i < 23 - countNewLine; i++)
            {
                SystemIO.STDOUT.PutLine("");
                SystemIO.STDOUT.Put("~");
            }

            //PRINT INSTRUCTION
            SystemIO.STDOUT.PutLine("");
            for (int i = 0; i < 72; i++)
            {
                if (i < infoBar.Length)
                {
                    SystemIO.STDOUT.Put(infoBar[i]);
                }
                else
                {
                    SystemIO.STDOUT.Put(" ");
                }
            }

            if (editMode)
            {
                SystemIO.STDOUT.Put(countNewLine + 1 + "," + countChars);
            }

        }

        public static String miv(String start)
        {
            Boolean editMode = false;
            int pos = 0;
            char[] chars = new char[2000];
            String infoBar = String.Empty;

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
                                String returnString = String.Empty;
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
                    infoBar = String.Empty;
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
            GlobalData.FileToEdit = path;
            try
            {
                if (File.Exists(GlobalData.CurrentDirectory + GlobalData.FileToEdit))
                {
                    SystemIO.STDOUT.PutLine("Found file!");
                }
                else if (!File.Exists(GlobalData.CurrentDirectory + GlobalData.FileToEdit))
                {
                    SystemIO.STDOUT.PutLine("Creating file!");
                    File.Create(GlobalData.CurrentDirectory + GlobalData.FileToEdit);
                }
                if (WinttOS.IsTty)
                    WinttOS.Tty.ClearText();
                else
                    Console.Clear();
            }
            catch (Exception ex)
            {
                SystemIO.STDOUT.PutLine(ex.Message);
            }

            String text = String.Empty;
            SystemIO.STDOUT.PutLine("Do you want to open " + GlobalData.FileToEdit + " content? (Yes/No)");
            if (SystemIO.STDIN.Get().ToLower() == "yes" || SystemIO.STDIN.Get().ToLower() == "y")
            {
                text = miv(File.ReadAllText(GlobalData.CurrentDirectory + GlobalData.FileToEdit));
            }
            else
            {
                text = miv(null);
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