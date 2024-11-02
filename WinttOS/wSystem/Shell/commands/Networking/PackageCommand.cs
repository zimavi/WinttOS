using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class PackageCommand : Command
    {
        public PackageCommand(string[] commandValues) : base(commandValues, AccessLevel.Administrator)
        { }

        public override ReturnInfo Execute()
        {
            PrintHelp();
            return new(this, ReturnCode.OK);
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count == 0 || arguments.Count > 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
            }

            try
            {
                string command = arguments[0];

                if (command == "update")
                {
                    PackageManager.Update();

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "upgrade")
                {
                    PackageManager.Upgrade();

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "list")
                {
                    if (PackageManager.LocalRepository.Count == 0)
                    {
                        SystemIO.STDOUT.PutLine("No package found! Please make 'apt-get update' to update the package list.");
                        return new ReturnInfo(this, ReturnCode.OK);
                    }
                    else
                    {
                        SystemIO.STDOUT.PutLine("Package list:");

                        foreach (var package in PackageManager.LocalRepository)
                        {
                            SystemIO.STDOUT.PutLine("- " + package.Name + " v" + package.Version + " (by " + package.Author + "), " + (package.Installed ? "installed." : "not installed."));
                            SystemIO.STDOUT.PutLine("\t" + package.Description);
                        }

                        return new ReturnInfo(this, ReturnCode.OK);
                    }
                }
                else if (command == "install")
                {
                    if (arguments.Count != 2)
                    {
                        return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
                    }

                    var packageName = arguments[1];

                    PackageManager.Install(packageName);

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "remove")
                {
                    if (arguments.Count != 2)
                    {
                        return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
                    }

                    var packageName = arguments[1];

                    PackageManager.Remove(packageName);

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else
                {
                    return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Unknown package command.");
                }
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.ToString());
            }
        }
    }
}
