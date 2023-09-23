using static WinttOS.System.Processing.Process;

namespace WinttOS.System
{
    public static class Extensions
    {
        public static string ToString(this ProcessType type)
        {
            #pragma warning disable CS8524

            return type switch
            {
                ProcessType.KernelComponent => "KernelComponent",
                ProcessType.Driver => "Driver",
                ProcessType.Service => "Service",
                ProcessType.Program => "Program",
            };

            #pragma warning restore CS8524
        }
    }
}
