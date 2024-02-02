﻿using System;
using System.Collections.Generic;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Misc
{
    public class ShutdownCommand : Command
    {

        public ShutdownCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            CommandManual = new List<string>()
            {
                "Shutdown command is used for managing power in your PC",
                "It has to have one of two arguments '--shutdown' or '--reboot'",
                "'--shutdown' argument shuts PC down, and '--reboot' reboots it"
            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--shutdown" || arguments[0] == "-s")
            {
                Kernel.ShutdownKernel();
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--reboot" || arguments[0] == "-r")
            {
                Kernel.RebootKernel();
                return new(this, ReturnCode.OK);
            }
            else
            {
                return new(this, ReturnCode.OK, "Unknown argument!");
            }
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- shutdown {--shutdown|--reboot}");
        }
    }
}
