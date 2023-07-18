﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    public class ClearScreenCommand : Command
    {
        public ClearScreenCommand(string name) : base(name)
        {

        }
        public override string execute(string[] arguments)
        {
            Console.Clear();
            return string.Empty;
        }
    }
}