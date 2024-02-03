﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.Shell.Programs
{
    internal class SandboxingTest : Process
    {
        public SandboxingTest() : base("SomeVirus", ProcessType.KernelComponent)
        {
        }

        public override void Start()
        {
            base.Start();

            if (TryRisePrivileges(API.PrivilegesSystem.PrivilegesSet.RAISED))
                WinttDebugger.Debug("SandboxTest -> Raised >:3");
            else
                WinttDebugger.Debug("SandboxTest -> Not raised ;3");
        }
    }
}