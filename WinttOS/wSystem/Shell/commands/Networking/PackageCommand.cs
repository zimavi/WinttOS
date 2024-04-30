using System;
using System.Collections.Generic;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class PackageCommand : Command
    {
        public PackageCommand(string[] commandValues) : base(commandValues, Users.User.AccessLevel.Administrator)
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
                    WinttOS.PackageManager.Update();

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "upgrade")
                {
                    WinttOS.PackageManager.Upgrade();

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "list")
                {
                    if (WinttOS.PackageManager.LocalRepository.Count == 0)
                    {
                        Console.WriteLine("No package found! Please make 'apt-get update' to update the package list.");
                        return new ReturnInfo(this, ReturnCode.OK);
                    }
                    else
                    {
                        Console.WriteLine("Package list:");

                        foreach (var package in WinttOS.PackageManager.LocalRepository)
                        {
                            Console.WriteLine("- " + package.Name + " v" + package.Version + " (by " + package.Author + "), " + (package.Installed ? "installed." : "not installed."));
                            Console.WriteLine("\t" + package.Description);
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

                    WinttOS.PackageManager.Install(packageName);

                    return new ReturnInfo(this, ReturnCode.OK);
                }
                else if (command == "remove")
                {
                    if (arguments.Count != 2)
                    {
                        return new ReturnInfo(this, ReturnCode.ERROR_ARG, "Expected 2 values!");
                    }

                    var packageName = arguments[1];

                    WinttOS.PackageManager.Remove(packageName);

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
