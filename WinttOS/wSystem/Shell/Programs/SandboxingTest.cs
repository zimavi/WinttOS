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
        }
    }
}
