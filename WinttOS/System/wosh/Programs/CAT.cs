using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core;

namespace WinttOS.System.wosh.Programs
{
    public class CAT
    {
        public string Execute(string[] arguments)
        {
            string text = "";
            if (arguments.Length == 0)
                return "Usage: cat <path\to\file>";
            else if (arguments.Length == 1)
                text = File.ReadAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[0]);
            else if (arguments.Length > 1)
            {
                if (arguments[0] == "-n")
                {
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        text += $"\t{i + 1} {File.ReadAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[i])}";
                    }
                }
                else if (arguments[0] == ">")
                {
                    text = Console.ReadLine();
                    File.WriteAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[1], text);
                    return string.Empty;
                }
                else if (arguments[1] == ">")
                {
                    if (File.Exists(@"0:\" + GlobalData.CurrentDirectory + arguments[0]))
                    {
                        text = File.ReadAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[0]);
                    }
                    else
                        return "Files " + @"0:\" + GlobalData.CurrentDirectory + arguments[0] + " does not exists!";
                    if (File.Exists(@"0:\" + GlobalData.CurrentDirectory + arguments[2]))
                    {
                        if (Kernel.ReadonlyFiles.Contains(@"0:\" + GlobalData.CurrentDirectory + arguments[2]))
                            return "Files is readonly!";
                        File.WriteAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[2], text);
                    }
                    text = "The content will be copied in destination file";
                }
                else if (arguments[1] == ">>")
                {
                    if (File.Exists(@"0:\" + GlobalData.CurrentDirectory + arguments[0]))
                    {
                        text = File.ReadAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[0]);
                    }
                    else
                        return "Files " + @"0:\" + GlobalData.CurrentDirectory + arguments[0] + " does not exists!";
                    if (File.Exists(@"0:\" + GlobalData.CurrentDirectory + arguments[2]))
                    {
                        if (Kernel.ReadonlyFiles.Contains(@"0:\" + GlobalData.CurrentDirectory + arguments[2]))
                            return "Files is readonly!";
                        File.AppendAllText(@"0:\" + GlobalData.CurrentDirectory + arguments[2], text);
                    }
                    text = "The content will be copied in destination file";
                }
                else
                {
                    foreach (string str in arguments)
                    {
                        text += File.ReadAllText(@"0:\" + GlobalData.CurrentDirectory + str);
                    }
                }
            }
            return text;
        }
    }
}
