using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Processing;

namespace WinttOS.System.Shell.Programs
{
    internal class Sandbox : Process
    {

        private string targetName;
        private Process target;
        private uint targetPID;

        public Sandbox(string sandboxTarget) : base($"Sandbox ({sandboxTarget})", ProcessType.Program)
        {
            targetName = sandboxTarget;
        }

        public override void Start()
        {
            base.Start();

            target = new SandboxingTest();

            SetChild(target);

            if(!TrySetChildPrivileges(target, API.PrivilegesSystem.PrivilegesSet.NONE))
            {
                WinttOS.ProcessManager.TryStopProcess(ProcessID);
            }

            if (!WinttOS.ProcessManager.TryRegisterProcess(target, out targetPID))
            {
                WinttOS.ProcessManager.TryStopProcess(ProcessID);
            }

            WinttOS.ProcessManager.TryStartProcess(targetPID);
        }

        public override void Stop()
        {
            base.Stop();

            WinttOS.ProcessManager.TryStopProcess(targetPID);
        }
    }
}
