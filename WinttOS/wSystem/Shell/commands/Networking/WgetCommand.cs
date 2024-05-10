using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Networking;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class WgetCommand : Command
    {
        public WgetCommand(string[] name) : base(name)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            string url = arguments[0];

            if (url.StartsWith("https://"))
            {
                return new(this, ReturnCode.ERROR_ARG, "HTTPS currently not supported, please use HTTP");
            }

            try
            {
                string file = Http.DownloadFile(url);

                File.WriteAllText(GlobalData.CurrentDirectory + "file.html", file);

                SystemIO.STDOUT.PutLine(url + " saved to file.html");
                return new(this, ReturnCode.OK);
            }
            catch(Exception e)
            {
                return new(this, ReturnCode.CRASH, e.Message);
            }
        }
    }
}
