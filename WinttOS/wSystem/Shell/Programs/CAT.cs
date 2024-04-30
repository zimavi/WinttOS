using System;
using System.IO;
using WinttOS.Core;

namespace WinttOS.wSystem.Shell.Programs
{
    public sealed class CAT
    {
        public string Execute(string[] arguments)
        {
            string text = "";
            if (arguments.Length == 0)
                return "Usage: cat <path\\to\\file>";
            else if (arguments.Length == 1)
                text = File.ReadAllText(GlobalData.CurrentDirectory + arguments[0]);
            else if (arguments.Length > 1)
            {
                if (arguments[0] == "-n")
                {
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        text += $"\t{i + 1} {File.ReadLines(GlobalData.CurrentDirectory + arguments[i])}";
                    }
                }
                else if (arguments[0] == ">")
                {
                    text = Console.ReadLine();
                    File.WriteAllText(GlobalData.CurrentDirectory + arguments[1], text);
                    return string.Empty;
                }
                else if (arguments[1] == ">")
                {
                    if (File.Exists(GlobalData.CurrentDirectory + arguments[0]))
                    {
                        text = File.ReadAllText(GlobalData.CurrentDirectory + arguments[0]);
                    }
                    else
                        return "Files " + GlobalData.CurrentDirectory + arguments[0] + " does not exists!";
                    if (File.Exists(GlobalData.CurrentDirectory + arguments[2]))
                    {
                        if (Kernel.ReadonlyFiles.Contains(GlobalData.CurrentDirectory + arguments[2]))
                            return "Files is readonly!";
                        File.WriteAllText(GlobalData.CurrentDirectory + arguments[2], text);
                    }
                    text = "The content will be copied in destination file";
                }
                else if (arguments[1] == ">>")
                {
                    if (File.Exists(GlobalData.CurrentDirectory + arguments[0]))
                    {
                        text = File.ReadAllText(GlobalData.CurrentDirectory + arguments[0]);
                    }
                    else
                        return "Files " + GlobalData.CurrentDirectory + arguments[0] + " does not exists!";
                    if (File.Exists(GlobalData.CurrentDirectory + arguments[2]))
                    {
                        if (Kernel.ReadonlyFiles.Contains(GlobalData.CurrentDirectory + arguments[2]))
                            return "Files is readonly!";
                        File.AppendAllText(GlobalData.CurrentDirectory + arguments[2], text);
                    }
                    text = "The content will be copied in destination file";
                }
                else
                {
                    foreach (string str in arguments)
                    {
                        text += File.ReadAllText(GlobalData.CurrentDirectory + str);
                    }
                }
            }
            return text;
        }
    }
}
