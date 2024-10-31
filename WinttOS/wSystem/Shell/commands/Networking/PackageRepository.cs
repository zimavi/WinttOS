using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class PackageRepository : Command
    {
        public PackageRepository(string[] commandValues) : base(commandValues, AccessLevel.Administrator)
        { }


        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count == 0 || arguments.Count > 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);
            }

            try
            {
                string command = arguments[0];

                if (command == "add")
                {
                    if (arguments.Count != 2)
                    {
                        return new(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
                    }

                    WinttOS.PackageManager.AddRepo(arguments[1]);
                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if(command == "list")
                {
                    SystemIO.STDOUT.PutLine("List of packages repositories:");
                    foreach(string repoLink in WinttOS.PackageManager.Repositories)
                    {
                        SystemIO.STDOUT.PutLine("- " + repoLink);
                    }

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "remove")
                {
                    if (arguments.Count != 2)
                    {
                        return new(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
                    }
                    if (int.TryParse(arguments[1], out int id))
                    {
                        WinttOS.PackageManager.RemoveRepo(id);
                    }
                    else
                    {
                        return new ReturnInfo(this, ReturnCode.ERROR, "Invalid Integer");
                    }
                    return new ReturnInfo(this, ReturnCode.OK);
                }
                return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Unknown operation!");
            }
            catch(Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG, ex.ToString());
            }
        }
    }
}
