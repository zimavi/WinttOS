using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    //
    // CAT VERISON 0.1.0
    //
    public class CatUtilCommand : Command
    {
        public CatUtilCommand(string name) : base(name)
        {
            HelpCommandManager.addCommandUageStrToManager(@"cat <path\to\file> - reads all text from file (use 'man cat' for more info)");
        }

        public override string execute(string[] arguments)
        {
            string text = "";
            if (arguments.Length == 0)
                return "Usage: cat <path\to\file>";
            else if (arguments.Length == 1)
                text = File.ReadAllText(@"0:\" + GlobalData.currDir + arguments[0]);
            else if (arguments.Length > 1)
            {
                if (arguments[0] == "-n")
                {
                    for(int i = 0; i < arguments.Length; i++)
                    {
                        text += $"\t{i + 1} {File.ReadAllText(@"0:\" + GlobalData.currDir + arguments[i])}";
                    }
                }
                else if (arguments[0] == ">")
                {
                    text = Console.ReadLine();
                    File.WriteAllText(@"0:\" + GlobalData.currDir + arguments[1], text);
                    return String.Empty;
                }
                else if (arguments[1] == ">")
                {
                    if (File.Exists(@"0:\" + GlobalData.currDir + arguments[0]))
                    {
                        text = File.ReadAllText(@"0:\" + GlobalData.currDir + arguments[0]);
                    }
                    else
                        return "File " + @"0:\" + GlobalData.currDir + arguments[0] + " does not exists!";
                    if (File.Exists(@"0:\" + GlobalData.currDir + arguments[2]))
                        File.WriteAllText(@"0:\" + GlobalData.currDir + arguments[2], text);
                    text = "The content will be copied in destination file";
                }
                else if (arguments[1] == ">>")
                {
                    if (File.Exists(@"0:\" + GlobalData.currDir + arguments[0]))
                    {
                        text = File.ReadAllText(@"0:\" + GlobalData.currDir + arguments[0]);
                    }
                    else
                        return "File " + @"0:\" + GlobalData.currDir + arguments[0] + " does not exists!";
                    if (File.Exists(@"0:\" + GlobalData.currDir + arguments[2]))
                        File.AppendAllText(@"0:\" + GlobalData.currDir + arguments[2], text);
                    text = "The content will be copied in destination file";
                }
                else
                {
                    foreach(string str in arguments)
                    {
                        text += File.ReadAllText(@"0:\" + GlobalData.currDir + str);
                    }
                }
            }
            return text;
        }
    }
}
