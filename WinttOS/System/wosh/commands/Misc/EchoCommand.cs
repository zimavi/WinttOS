using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.wosh;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.Misc
{
    public class EchoCommand : Command
    {
        public EchoCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"echo <message> - repeats message");
            CommandManual = new List<string>()
            {
                "Echo command return all text that was gave after 'echo'",
                "For example:",
                "Console input 'echo Hello World!' will return 'Hello World!' message"
            };
        }

        public override string Execute(string[] arguments)
        {
            string response = "";

            if (arguments.Length != 0)
            {
                string str = "";
                foreach (string args in arguments)
                {
                    str += args + " ";
                }
                if (str.Contains("$"))
                {
                    string St = str;

                    int pFrom = St.IndexOf("$") + "$".Length;
                    int pTo = St.LastIndexOf(" ");

                    string result = St.Substring(pFrom, pTo - pFrom);
                    DateTime dateTime = new DateTime(DateTime.Now.Year, 8, 8);
                    DateTime CurrentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    int IsEastereggIsDetected = DateTime.Compare(dateTime, CurrentDateTime);
                    if (result == "DuCa") //=)
                    {
                        if (IsEastereggIsDetected != -1)
                        { // easteregg = True #Cuz -1 is not equal to current time =)
                            Console.WriteLine("Variable: " + result);
                            Console.WriteLine(@"
  (\___/)         Welcome, this is rabit =)
  (='.'=)         and yes, if you are in Vietnamese, you already know about 
 ('')_('')        'Hibiki Duca' right ?
                                                        - Variable $DuCa -");
                            return "";
                        }
                        else
                        {
                            Console.WriteLine("WARNING: You need to wait to 8/8 to this easter egg work =)");
                        }
                    }
                    else
                    {
                        response = "Variable: " + result;
                    }
                    return "";
                }

                response = string.Format("{0}", str);
            }
            else
            {
                response = "ECHO is off by default.";
            }

            return response;
        }
    }
}
