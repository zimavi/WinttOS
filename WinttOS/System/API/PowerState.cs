using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.API
{
    public static class PowerState
    {
        public static void RequestSystemShutdown()
        {
            Kernel.ShutdownKernel();
        }
    }
}
