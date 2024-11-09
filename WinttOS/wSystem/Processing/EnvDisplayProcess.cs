using WinttOS.wSystem.IO;
using WinttOS.wSystem.Registry;

namespace WinttOS.wSystem.Processing
{
    internal class EnvDisplayProcess : Process
    {
        public EnvDisplayProcess() : base("envdisplay", ProcessType.Program)
        {}

        public override void Start()
        {
            base.Start();

            SystemIO.STDOUT.PutLine("");
            SystemIO.STDOUT.PutLine("");
            SystemIO.STDOUT.PutLine("Global environment");
            foreach (var key in Environment.GlobalEnvironment)
            {
                SystemIO.STDOUT.PutLine($"{key.Name} = {key.Value}");
            }

            SystemIO.STDOUT.PutLine("Process environment");
            foreach (var key in Environment.PerProcessEnvironment[(int)ProcessID])
            {
                SystemIO.STDOUT.PutLine($"{key.Name} = {key.Value}");
            }

            Exit(0);
        }
    }
}
