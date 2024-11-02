using System;
using System.Collections.Generic;
using UniLua;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
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

                    if (arguments[1].StartsWith("https://"))
                    {
                        SystemIO.STDOUT.PutLine("HTTPS is not supported yet, please use HTTP");
                        return new(this, ReturnCode.ERROR);
                    }

                    PackageManager.Repositories.Add(arguments[1]);

                    SystemIO.STDOUT.PutLine("Done.");
                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if(command == "list")
                {
                    SystemIO.STDOUT.PutLine("List of packages repositories:");
                    foreach(string repoLink in PackageManager.Repositories)
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
                        PackageManager.RemoveRepo(id - 1);
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
