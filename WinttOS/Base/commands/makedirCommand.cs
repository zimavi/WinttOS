﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    public class makedirCommand : Command
    {
        public makedirCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            if(arguments.Length >= 1)
            {
                GlobalData.fs.CreateDirectory(@"0:\" + GlobalData.currDir + arguments[0]);
            }
            else
            {
                Console.Write("Enter new dir name: ");
                string dir = Console.ReadLine();
                GlobalData.fs.CreateDirectory(@"0:\" + GlobalData.currDir + dir);
            }
            return "Created directory!";
        }
    }
}