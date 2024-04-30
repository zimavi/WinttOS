using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.Shell.Programs
{
    internal sealed class SandboxingTest : Process
    {
        public SandboxingTest() : base("SomeVirus", ProcessType.KernelComponent)
        {
        }

        public override void Start()
        {
            base.Start();

            if (TryRisePrivileges(wAPI.PrivilegesSystem.PrivilegesSet.RAISED))
                WinttDebugger.Debug("SandboxTest -> Raised >:3");
            else
                WinttDebugger.Debug("SandboxTest -> Not raised ;3");
        }
    }
}
