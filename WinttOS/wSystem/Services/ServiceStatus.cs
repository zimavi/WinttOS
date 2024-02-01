using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Services
{
    [Flags]
    public enum ServiceStatus : byte
    {
        no_data = 0,
        OK = 1,
        OFF = 2,
        PAUSED = 4,
        PENDING = 8,
        ERROR = 16,
    }
}
