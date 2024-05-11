﻿using System;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class RmCommand : Command
    {
        public RmCommand(string[] name) : base(name, User.AccessLevel.Guest)
        { }
        public override ReturnInfo Execute(List<string> arguments)
        {
            string path = GlobalData.CurrentDirectory + arguments[0];

            if(!Entries.ForceRemove(path))
            {
                return new(this, ReturnCode.ERROR);
            }

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("rm [file or directory]");
        }
    }
}
