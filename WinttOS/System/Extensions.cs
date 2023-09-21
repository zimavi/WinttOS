using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinttOS.System.Processing.Process;

namespace WinttOS.System
{
    public static class Extensions
    {
        public static string ToString(this Processing.Process.ProcessType type)
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
