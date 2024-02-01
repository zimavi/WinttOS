using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core;
using WinttOS.wSystem.Networking;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public class WgetCommand : Command
    {
        public WgetCommand(string name) : base(name)
        { }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                return "Usage: wget {url}";
            }

            string url = arguments[0];

            if (url.StartsWith("https://"))
            {
                return "HTTPS currently not supported, please use HTTP";
            }

            try
            {
                string file = Http.DownloadFile(url);

                File.WriteAllText(GlobalData.CurrentDirectory + "file.html", file);

                return url + " saved to file.html";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
